using FluentAssertions;
using QuokkaDev.AsyncNotifications.Abstractions;
using QuokkaDev.AsyncNotifications.Abstractions.Exceptions;
using QuokkaDev.AsyncNotifications.Tests.Utilities;
using Moq;
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
        context = new DependencyInjectionContext();
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

        try
        {
            await asyncNotificationDispatcher.DispatchAsync(notification);
        }
        catch(AggregateException agex)
        {
        }

        // Assert
        methodCallMock.Verify(caller => caller.Call(), Times.Exactly(3));
    }

    [Fact]
    public async Task A_NotificationException_Should_Be_Raised()
    {
        // Arrange
        var notification = new Notification1();

        // Act
        var dispatch = async() => await asyncNotificationDispatcher.DispatchAsync(notification);

        // Assert
        await dispatch.Should().ThrowAsync<NotificationException>();
    }
}