using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Abstractions
{
    /// <summary>
    /// Interface for async notification dispatcher
    /// </summary>
    public interface IAsyncNotificationDispatcher
    {
        /// <summary>
        /// Dispatch a notification to the right handlers
        /// </summary>
        /// <typeparam name="T">The type of the notification</typeparam>
        /// <param name="notification">The notification</param>
        /// <param name="cancellation">A cancellation token</param>
        Task DispatchAsync<T>(T notification, CancellationToken cancellation);
        /// <summary>
        /// Dispatch a notification to the right handlers
        /// </summary>
        /// <typeparam name="T">The type of the notification</typeparam>
        /// <param name="notification">The notification</param>
        Task DispatchAsync<T>(T notification);
    }
}
