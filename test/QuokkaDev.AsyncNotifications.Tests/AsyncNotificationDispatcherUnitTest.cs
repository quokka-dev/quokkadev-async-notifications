using FluentAssertions;
using Moq;
using QuokkaDev.AsyncNotifications.Abstractions;
using QuokkaDev.AsyncNotifications.Abstractions.Exceptions;
using QuokkaDev.AsyncNotifications.Tests.Utilities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace QuokkaDev.AsyncNotifications.Tests;

public class AsyncNotificationDispatcherUnitTest
{
    private readonly Mock<IMethodCall> methodCallMock;
    private readonly IAsyncNotificationDispatcher asyncNotificationDispatcher;
    private readonly DependencyInjectionContext context;

    public AsyncNotificationDispatcherUnitTest()
    {
        context = new DependencyInjectionContext(typeof(DependencyInjectionContext).Assembly);
        methodCallMock = context.RegisterMethodCall();
        context.BuildServiceProvider();
        asyncNotificationDispatcher = context.GetService<IAsyncNotificationDispatcher>();
    }

    [Fact]
    public async Task Exactly_3_Handlers_Should_Be_Invoked()
    {
        // Arrange
        var notification = new Notification1();

        // Act
        await asyncNotificationDispatcher.DispatchAsync(notification);

        // Assert
        methodCallMock.Verify(caller => caller.Call(), Times.Exactly(3));
    }

    [Fact]
    public async Task A_NotificationException_Should_Be_Raised()
    {
        // Arrange
        var notification = new Notification1() { ThrowException = true };

        // Act
        var dispatch = async () => await asyncNotificationDispatcher.DispatchAsync(notification);

        // Assert
        await dispatch.Should().ThrowAsync<NotificationException>();
    }

    [Fact]
    public async Task Current_Assembly_Should_Be_Used_If_Not_Supplied()
    {
        // Arrange
        var localContext = new DependencyInjectionContext();
        var localMethodCallMock = localContext.RegisterMethodCall();
        localContext.BuildServiceProvider();
        var dispatcher = localContext.GetService<IAsyncNotificationDispatcher>();
        var notification = new Notification1();

        // Act
        await dispatcher.DispatchAsync(notification);

        // Assert
        localMethodCallMock.Verify(caller => caller.Call(), Times.Exactly(3));
    }

    [Fact]
    public async Task Exactly_3_Handlers_Should_Be_Invoked_For_Concrete_Types()
    {
        // Arrange
        INotification notification = new Notification1();

        // Act

        try
        {
            await asyncNotificationDispatcher.DispatchAsync(notification);
        }
        catch(AggregateException)
        {
        }

        // Assert
        methodCallMock.Verify(caller => caller.Call(), Times.Exactly(3));
    }
}