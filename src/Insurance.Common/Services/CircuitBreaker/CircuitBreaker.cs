namespace Insurance.Common.Services.CircuitBreaker
{
    public class CircuitBreaker
    {
        private readonly CircuitBreakerAction action;
        private readonly CircuitBreakerOption option;

        private int failureOccurrences;
        private bool _isOpen;
        private DateTime lastFailureOccurrence;


        public CircuitBreaker(CircuitBreakerAction action,
            CircuitBreakerOption? option = null)
        {
            this.action = action;
            this.option = option ?? new CircuitBreakerOption();
        }


        bool IsOpen
        {
            get
            {
                if (_isOpen)
                {
                    var difference = DateTime.Now - lastFailureOccurrence;
                    if (difference.TotalSeconds >= option.HealingPeriodInSec)
                    {
                        // after healing period, it gets back to normal

                        failureOccurrences = 0;
                        _isOpen = false;
                    }
                }

                return _isOpen;
            }
        }


        public async Task<object?> ExecuteAsync(object? arg)
        {
            try
            {
                if (IsOpen)
                {
                    var metadata = string.IsNullOrWhiteSpace(action.Metadata) ? "Not Available" : action.Metadata;
                    var msg = $"CircuitBreaker is open - Action.Id: {action.Id}, Action.MetaData: {metadata}";
                    throw new CircuitBreakerOpenException(msg);
                }

                return await action.ActAsync(arg);
            }
            catch (Exception ex)
            {
                if (ex is CircuitBreakerOpenException)
                    throw;

                lastFailureOccurrence = DateTime.Now;

                if (++failureOccurrences >= option.FailureThreshold)
                {
                    _isOpen = true;
                }

                throw;
            }
        }
    }
}
