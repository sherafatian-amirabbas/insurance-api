using System.Net;
using System.Runtime.Serialization;

namespace Insurance.Contracts.Application.Exceptions
{
    [Serializable]
    public class BusinessException : AppException
    {
        public BusinessException(string message,
            HttpStatusCode statusCode)
        : base(message, statusCode)
        { }

        protected BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
        { }
    }
}
