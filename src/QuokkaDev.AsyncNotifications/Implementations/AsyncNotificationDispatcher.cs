using Microsoft.Extensions.DependencyInjection;
using QuokkaDev.AsyncNotifications.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Implementations
{
    internal class AsyncNotificationDispatcher : IAsyncNotificationDispatcher
    {
        private readonly IServiceProvider serviceProvider;
        private readonly List<Exception> aggregateException = new List<Exception>();

        public AsyncNotificationDispatcher(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task DispatchAsync<T>(T notification, CancellationToken cancellation)
        {
            var handlers = serviceProvider.GetServices<INotificationHandler<T>>();
            var tasks = handlers.Select(handler => BufferCall(handler, notification, cancellation));
            await Task.WhenAll(tasks);

            if(this.aggregateException.Any()) {
                throw new AggregateException(this.aggregateException);
            }
        }

        public Task DispatchAsync<T>(T notification)
        {
            return DispatchAsync<T>(notification, CancellationToken.None);
        }

        private async Task BufferCall<T>(INotificationHandler<T> handler, T notification, CancellationToken cancellation)
        {
            try {
                await handler.Handle(notification, cancellation);
            }
            catch(Exception e) {
                this.aggregateException.Add(e);
            }
        }
    }
}
