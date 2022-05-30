using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuokkaDev.AsyncNotifications.Abstractions.Exceptions
{
    /// <summary>
    /// Exception throwed by notification handlers
    /// </summary>
    public class NotificationException : ApplicationException
    {
        public NotificationException() : base()
        {
        }

        public NotificationException(string? message) : base(message)
        {
        }

        public NotificationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
