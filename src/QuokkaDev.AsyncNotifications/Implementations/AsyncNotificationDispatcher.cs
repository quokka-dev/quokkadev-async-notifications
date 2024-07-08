using QuokkaDev.AsyncNotifications.Abstractions;
using System.Collections.Concurrent;

namespace QuokkaDev.AsyncNotifications.Implementations
{
    internal class AsyncNotificationDispatcher : IAsyncNotificationDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private static ConcurrentDictionary<Type, AbstractHandlerExecutor> executorDictionary = new();

        public AsyncNotificationDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<T>(T notification, CancellationToken cancellation)
        {
            Type? notificationType = notification?.GetType();
            if(notificationType != null)
            {
                var executor = executorDictionary.GetOrAdd(notificationType, static type =>
                {
                    var concreteType = typeof(HandlerExecutor<>).MakeGenericType(type);
                    return (AbstractHandlerExecutor)Activator.CreateInstance(concreteType)!;
                });

                await executor.HandleAsync(notification!, serviceProvider, cancellation);
            }
        }

        public Task DispatchAsync<T>(T notification)
        {
            return DispatchAsync<T>(notification, CancellationToken.None);
        }
    }
}
