using Insurance.Common.Services.CircuitBreaker;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using System;
using Xunit;
using common = Insurance.Common.Services.CircuitBreaker;

namespace Insurance.Tests.Common.Services.CircuitBreaker
{
    public class CircuitBreakerTests
    {
        [Fact]
        public async Task ExecuteAsync_ThresholdIsNotReached_ActionGetsResultBackSuccessfully()
        {
            // arrange
            var id = Guid.NewGuid().ToString();
            var expectedResult = new object();
            var action = new common.CircuitBreakerAction(id, async () => await Task.FromResult(expectedResult));
            var option = new CircuitBreakerOption(2, 3);
            var cBreaker = new common.CircuitBreaker(action, option);

            // act
            var result = await cBreaker.ExecuteAsync();

            // assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task ExecuteAsync_ThresholdIsReached_ActionIsNotExecuted()
        {
            // arrange
            var exceptionMessage = "connection broken!";
            var id = Guid.NewGuid().ToString();
            var expectedResult = new object();
            var action = new common.CircuitBreakerAction(id, async () =>
            {
                throw new Exception(exceptionMessage);
                return await Task.FromResult(expectedResult);
            });
            var option = new CircuitBreakerOption(2, 3);
            var cBreaker = new common.CircuitBreaker(action, option);

            // act and assert
            _ = await Assert.ThrowsAsync<Exception>(cBreaker.ExecuteAsync);
            _ = await Assert.ThrowsAsync<Exception>(cBreaker.ExecuteAsync);
            _ = await Assert.ThrowsAsync<CircuitBreakerOpenException>(cBreaker.ExecuteAsync);
        }

        [Fact]
        public async Task ExecuteAsync_ThresholdIsReached_ActionGetsExecutedAfterHealingPeriod()
        {
            // arrange
            var exceptionMessage = "connection broken!";
            var id = Guid.NewGuid().ToString();
            var expectedResult = new object();
            var action = new common.CircuitBreakerAction(id, async () =>
            {
                throw new Exception(exceptionMessage);
                return await Task.FromResult(expectedResult);
            });
            var option = new CircuitBreakerOption(2, 3);
            var cBreaker = new common.CircuitBreaker(action, option);

            // arrange and assert
            _ = await Assert.ThrowsAsync<Exception>(cBreaker.ExecuteAsync);
            _ = await Assert.ThrowsAsync<Exception>(cBreaker.ExecuteAsync);

            await Task.Delay(TimeSpan.FromSeconds(option.HealingPeriodInSec));

            var ex = await Assert.ThrowsAsync<Exception>(cBreaker.ExecuteAsync);
            Assert.Equal(exceptionMessage, ex.Message);
        }
    }
}
