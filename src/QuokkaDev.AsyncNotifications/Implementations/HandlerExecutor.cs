using Microsoft.Extensions.DependencyInjection;
using QuokkaDev.AsyncNotifications.Abstractions;

namespace QuokkaDev.AsyncNotifications.Implementations
{
    internal abstract class AbstractHandlerExecutor
    {
        public abstract Task HandleAsync(object notification, IServiceProvider serviceProvider, CancellationToken cancellation);
    }


    internal class HandlerExecutor<T> : AbstractHandlerExecutor
    {
        public override async Task HandleAsync(object notification, IServiceProvider serviceProvider, CancellationToken cancellation)
        {
            List<Exception> aggregateException = new List<Exception>();

            var handlers = serviceProvider.GetServices<INotificationHandler<T>>();
            var tasks = handlers.Select(handler => BufferCall<T>(handler, (T)notification, aggregateException, cancellation));
            await Task.WhenAll(tasks);

            if(aggregateException.Any())
            {
                throw new AggregateException(aggregateException);
            }
        }

        private async Task BufferCall<T>(INotificationHandler<T> handler, T notification, List<Exception> aggregateException, CancellationToken cancellation)
        {
            try
            {
                await handler.Handle(notification, cancellation);
            }
            catch(Exception e)
            {
                aggregateException.Add(e);
            }
        }
    }
}
