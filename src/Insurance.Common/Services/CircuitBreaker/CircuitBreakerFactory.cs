using Microsoft.Extensions.Caching.Memory;
using Insurance.Common.Interfaces;

namespace Insurance.Common.Services.CircuitBreaker
{
    public class CircuitBreakerFactory : ICircuitBreakerFactory
    {
        private readonly static object lockObject = new object();

        private readonly IMemoryCache memoryCache;


        public CircuitBreakerFactory(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }


        public CircuitBreaker Create(CircuitBreakerAction action)
        {
            return SetOrGetFromCache(action);
        }


        #region Private Methods

        private CircuitBreaker SetOrGetFromCache(CircuitBreakerAction action)
        {
            if (!memoryCache.TryGetValue(action.Id, out CircuitBreaker? val) || val == null)
            {
                lock (lockObject)
                {
                    if (!memoryCache.TryGetValue(action.Id, out val) || val == null)
                    {
                        val = new CircuitBreaker(action);
                        return memoryCache.Set(action.Id, val);
                    }
                }
            }

            return val;
        }

        #endregion
    }
}
