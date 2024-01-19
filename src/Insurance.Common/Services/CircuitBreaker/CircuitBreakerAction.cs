namespace Insurance.Common.Services.CircuitBreaker
{
    public record CircuitBreakerAction(string Id, Func<Task<object?>> ActAsync, string Metadata = "");
}
