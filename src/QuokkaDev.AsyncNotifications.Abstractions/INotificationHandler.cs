using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Abstractions
{
    /// <summary>
    /// Generic notification handler
    /// </summary>
    /// <typeparam name="TNotification">The type of notification</typeparam>
    public interface INotificationHandler<in TNotification>
    {
        /// <summary>
        /// Handle the notification
        /// </summary>
        /// <param name="notification">The notification to handle</param>
        /// <param name="cancellation">A cancellation token for long running tasks</param>
        Task Handle(TNotification notification, CancellationToken cancellation);
    }
}
