using System.Net;
using System.Runtime.Serialization;

namespace Insurance.Contracts.Application.Exceptions
{
    [Serializable]
    public class AppException : Exception
    {
        public AppException(string message,
            HttpStatusCode statusCode)
        : base(message)
        {
            StatusCode = statusCode;
        }

        protected AppException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }

        public HttpStatusCode StatusCode { get; }
    }
}
