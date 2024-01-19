using System.Net;
using System.Runtime.Serialization;

namespace Insurance.Contracts.Application.Exceptions
{
    [Serializable]
    public class TechnicalException : AppException
    {
        public TechnicalException(string message,
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message, statusCode)
        { }

        protected TechnicalException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
