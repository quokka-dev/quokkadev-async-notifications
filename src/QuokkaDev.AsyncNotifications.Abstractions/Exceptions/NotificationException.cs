using System.Runtime.Serialization;

namespace QuokkaDev.AsyncNotifications.Abstractions.Exceptions
{
    /// <summary>
    /// Exception throwed by notification handlers
    /// </summary>
    [System.Serializable]
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

        protected NotificationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
            // No custom attribute to add in serialization
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            // No custom attribute to add in serialization
            base.GetObjectData(info, context);
        }
    }
}
