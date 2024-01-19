namespace Insurance.Common.Services.CircuitBreaker
{
    public record CircuitBreakerOption(int FailureThreshold = 3,
            int HealingPeriodInSec = 30);
}
