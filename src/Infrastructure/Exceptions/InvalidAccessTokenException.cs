using System.Runtime.Serialization;

namespace Infrastructure.Exceptions
{
    public sealed class InvalidAccessTokenException : Exception
    {
        public InvalidAccessTokenException()
        {
        }

        public InvalidAccessTokenException(string? message) : base(message)
        {
        }

        public InvalidAccessTokenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public InvalidAccessTokenException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
