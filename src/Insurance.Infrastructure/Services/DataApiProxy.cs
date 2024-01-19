using Insurance.Common.Extensions;
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
        const string PRODUCTS_ENDPOINT_PATH = "/products";

        const string PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE = "/product_types/{0}";
        const string PRODUCT_TYPE_ENDPOINT_PATH = "/product_types";

        private readonly ApplicationHttpClient httpClient;
        private readonly ICircuitBreakerFactory circuitBreakerFactory;


        public DataApiProxy(ApplicationHttpClient httpClient,
            ICircuitBreakerFactory circuitBreakerFactory)
        {
            this.httpClient = httpClient;
            this.circuitBreakerFactory = circuitBreakerFactory;
        }


        #region ProductComplete Public Methods

        /// <summary>
        /// returns a complex object including product and product type
        /// </summary>
        public async Task<ProductComplete> GetProductCompleteAsync(int productId)
        {
            var productComplete = new ProductComplete(productId);

            productComplete.Product = await GetProductAsync(productId);

            if (productComplete.Product == null)
                return productComplete;

            productComplete.ProductType = await GetProductTypeAsync(productComplete.Product.ProductTypeId);

            return productComplete;
        }

        /// <summary>
        /// returns a list of complex object including product and product type
        /// </summary>
        public async Task<List<ProductComplete>> GetProductCompletesAsync(List<int> productIds)
        {
            var products = await GetProductsAsync() ?? new List<Product>();
            var productTypes = await GetProductTypesAsync() ?? new List<ProductType>();

            var productCompletes = productIds
                .LeftJoin(products,
                    productId => productId,
                    product => product.Id,
                    (productId, product) => new ProductComplete(productId) { Product = product })
                .ToList();

            var result = productCompletes
                .LeftJoin(productTypes,
                    productComplete => productComplete.Product?.ProductTypeId,
                    productType => productType.Id,
                    (productComplete, productType) =>
                    {
                        productComplete.ProductType = productType;
                        return productComplete;
                    })
                .ToList();

            return result;
        }

        #endregion


        #region Products Public Methods

        public async Task<Product?> GetProductAsync(int id)
        {
            var functionName = nameof(GetProductAsync);
            return await ExecuteViaCircuitBreakerAsync<Product>(functionName,
                async (object? arg) =>
                {
                    var path = string.Format(PRODUCT_BY_ID_ENDPOINT_TEMPLATE, arg?.ToString());
                    return await this.httpClient.GetAsync<Product>(path);
                },
                id
            );
        }

        public async Task<List<Product>?> GetProductsAsync()
        {
            var functionName = nameof(GetProductsAsync);
            return await ExecuteViaCircuitBreakerAsync<List<Product>>(functionName,
                async (object? arg) =>
                {
                    return await this.httpClient.GetAsync<List<Product>>(PRODUCTS_ENDPOINT_PATH);
                }
            );
        }

        #endregion


        #region Product Types Public Methods

        public async Task<ProductType?> GetProductTypeAsync(int id)
        {
            var functionName = nameof(GetProductTypeAsync);
            return await ExecuteViaCircuitBreakerAsync<ProductType>(functionName,
                async (object? arg) =>
                {
                    var path = string.Format(PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE, arg?.ToString());
                    return await this.httpClient.GetAsync<ProductType>(path);
                },
                id
            );
        }

        public async Task<List<ProductType>?> GetProductTypesAsync()
        {
            var functionName = nameof(GetProductTypesAsync);
            return await ExecuteViaCircuitBreakerAsync<List<ProductType>>(functionName,
                async (object? arg) =>
                {
                    return await this.httpClient.GetAsync<List<ProductType>>(PRODUCT_TYPE_ENDPOINT_PATH);
                }
            );
        }

        #endregion


        #region Private Methods

        private async Task<TResponse?> ExecuteViaCircuitBreakerAsync<TResponse>(string functionName,
            Func<object?, Task<object?>> ActAsync,
            object? argument = null)
        {
            var functionId = GetFunctionId(functionName);
            var circuitBreaker = circuitBreakerFactory.Create(new CircuitBreakerAction(functionId, ActAsync));

            return (TResponse?)(await circuitBreaker.ExecuteAsync(argument));
        }

        private string GetFunctionId(string functionName)
        {
            return $"{this.GetType().FullName}.{functionName}";
        }

        #endregion
    }
}
