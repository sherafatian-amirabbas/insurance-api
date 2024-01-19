using Insurance.Common;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Infrastructure.Services
{
    public class DataApiProxy : IDataApiProxy
    {
        const string PRODUCT_BY_ID_ENDPOINT_TEMPLATE = "/products/{0}";
        const string PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE = "/product_types/{0}";

        private readonly ApplicationHttpClient httpClient;


        public DataApiProxy(ApplicationHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }


        #region Public Methods

        public async Task<Product?> GetProductAsync(int id)
        {
            var path = string.Format(PRODUCT_BY_ID_ENDPOINT_TEMPLATE, id);
            return await this.httpClient.GetAsync<Product>(path);
        }

        public async Task<ProductType?> GetProductTypeAsync(int id)
        {
            var path = string.Format(PRODUCT_TYPE_BY_ID_ENDPOINT_TEMPLATE, id);
            return await this.httpClient.GetAsync<ProductType>(path);
        }

        #endregion
    }
}
