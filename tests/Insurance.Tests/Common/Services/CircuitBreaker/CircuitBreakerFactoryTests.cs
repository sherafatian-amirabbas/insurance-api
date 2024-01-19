using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Xunit;
using common = Insurance.Common.Services.CircuitBreaker;

namespace Insurance.Tests.Common.Services.CircuitBreaker
{
    public class CircuitBreakerFactoryTests
    {
        private readonly MemoryCache memoryCache;
        private readonly common.CircuitBreakerFactory circuitBreakerFactory;


        public CircuitBreakerFactoryTests()
        {
            memoryCache = new MemoryCache(new MemoryCacheOptions());
            circuitBreakerFactory = new common.CircuitBreakerFactory(memoryCache);
        }


        [Fact]
        public void Create_CircuitBreakerIsAlreadyCreated_ItIsFetchedFromCache()
        {
            // arrange
            var id = Guid.NewGuid().ToString();
            var action = new common.CircuitBreakerAction(id, async (object? arg) => await Task.FromResult(new object()));
            var expectedCBreaker = new common.CircuitBreaker(action);
            memoryCache.Set(id, expectedCBreaker);

            // act
            var cBreaker = circuitBreakerFactory.Create(action);

            // assert
            Assert.Equal(expectedCBreaker, cBreaker);
        }

        [Fact]
        public void Create_CircuitBreakerIsNotAlreadyCreated_ItIsCreatedAndSetInTheCache()
        {
            // arrange
            var id = Guid.NewGuid().ToString();
            var action = new common.CircuitBreakerAction(id, async (object? arg) => await Task.FromResult(new object()));

            // act
            var expectedCBreaker = circuitBreakerFactory.Create(action);

            // assert
            var cBreaker = memoryCache.Get<common.CircuitBreaker>(id);
            Assert.Equal(expectedCBreaker, cBreaker);
        }
    }
}
