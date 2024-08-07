﻿using QuokkaDev.AsyncNotifications.Abstractions;
using QuokkaDev.AsyncNotifications.Abstractions.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Tests.Utilities
{
    public interface INotification
    {

    }

    public class Notification1 : INotification
    {
        public bool ThrowException { get; set; }
    }

    public class Notification2 : INotification
    {
    }

    public interface IMethodCall
    {
        void Call();
    }

    public class NotificationHandler1 : INotificationHandler<Notification1>
    {
        private readonly IMethodCall methodCall;

        public NotificationHandler1(IMethodCall methodCall)
        {
            this.methodCall = methodCall;
        }

        public Task Handle(Notification1 notification, CancellationToken cancellation)
        {
            methodCall.Call();
            return Task.CompletedTask;
        }
    }

    public class NotificationHandler1_1 : INotificationHandler<Notification1>
    {
        private readonly IMethodCall methodCall;

        public NotificationHandler1_1(IMethodCall methodCall)
        {
            this.methodCall = methodCall;
        }

        public Task Handle(Notification1 notification, CancellationToken cancellation)
        {
            methodCall.Call();
            return Task.CompletedTask;
        }
    }

    public class NotificationHandler1_WithException : INotificationHandler<Notification1>
    {
        private readonly IMethodCall methodCall;

        public NotificationHandler1_WithException(IMethodCall methodCall)
        {
            this.methodCall = methodCall;
        }

        public Task Handle(Notification1 notification, CancellationToken cancellation)
        {
            methodCall.Call();
            if(notification.ThrowException)
            {
                throw new NotificationException();
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }

    public class NotificationHandler2 : INotificationHandler<Notification2>
    {
        private readonly IMethodCall methodCall;

        public NotificationHandler2(IMethodCall methodCall)
        {
            this.methodCall = methodCall;
        }

        public Task Handle(Notification2 notification, CancellationToken cancellation)
        {
            methodCall.Call();
            return Task.CompletedTask;
        }
    }
}
