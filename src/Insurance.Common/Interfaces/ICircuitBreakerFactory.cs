using Insurance.Common.Services.CircuitBreaker;

namespace Insurance.Common.Interfaces
{
    public interface ICircuitBreakerFactory
    {
        CircuitBreaker Create(CircuitBreakerAction action);
    }
}
