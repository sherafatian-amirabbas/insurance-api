using Insurance.Common.Interfaces;
using Insurance.Common.Services;
using Insurance.Common.Services.CircuitBreaker;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Infrastructure.Services
{
    public class DataApiProxy : IDataApiProxy
    {
        const string PRODUCT_BY_ID_ENDPOINT_TEMPLATE = "/products/{0}";
        const string PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE = "/product_types/{0}";

        private readonly ApplicationHttpClient httpClient;
        private readonly ICircuitBreakerFactory circuitBreakerFactory;


        public DataApiProxy(ApplicationHttpClient httpClient,
            ICircuitBreakerFactory circuitBreakerFactory)
        {
            this.httpClient = httpClient;
            this.circuitBreakerFactory = circuitBreakerFactory;
        }


        #region Public Methods

        public async Task<Product?> GetProductAsync(int id)
        {
            var functionName = nameof(GetProductAsync);
            return await ExecuteAsync<Product>(functionName, 
                async () => {
                    var path = string.Format(PRODUCT_BY_ID_ENDPOINT_TEMPLATE, id);
                    return await this.httpClient.GetAsync<Product>(path);
                }
            );
        }

        public async Task<ProductType?> GetProductTypeAsync(int id)
        {
            var functionName = nameof(GetProductTypeAsync);
            return await ExecuteAsync<ProductType>(functionName,
                async () =>
                {
                    var path = string.Format(PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE, id);
                    return await this.httpClient.GetAsync<ProductType>(path);
                }
            );
        }

        #endregion


        #region Private Methods

        private async Task<TResponse?> ExecuteAsync<TResponse>(string functionName,
            Func<Task<object?>> ActAsync)
        {
            var functionId = GetFunctionId(functionName);
            var circuitBreaker = circuitBreakerFactory.Create(new CircuitBreakerAction(functionId, ActAsync));

            return (TResponse?)(await circuitBreaker.ExecuteAsync());
        }

        private string GetFunctionId(string functionName)
        {
            return $"{this.GetType().FullName}.{functionName}";
        }

        #endregion
    }
}
