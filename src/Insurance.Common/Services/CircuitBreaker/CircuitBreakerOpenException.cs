using System.Runtime.Serialization;

namespace Insurance.Common.Services.CircuitBreaker
{
    [Serializable]
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException(string message)
        : base(message)
        { }

        protected CircuitBreakerOpenException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        { }
    }
}
