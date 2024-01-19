namespace Insurance.Common.Services.CircuitBreaker
{
    public record CircuitBreakerAction(string Id, 
        Func<object?, Task<object?>> ActAsync,
        string Metadata = "");
}
