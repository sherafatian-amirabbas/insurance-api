using Insurance.Common.Extensions;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Specflow.Mock
{
    public class MockDataApiProxy : IDataApiProxy
    {
        List<Product> products;
        List<ProductType> productTypes;


        public MockDataApiProxy()
        {
            products = new();
            productTypes = new();
        }


        #region Public Methods

        public void SetProducts(List<Product> productList)
        {
            products = productList;
        }

        public void SetProductTypes(List<ProductType> productTypeList)
        {
            productTypes = productTypeList;
        }

        #endregion



        #region ProductComplete Public Methods

        public async Task<ProductComplete> GetProductCompleteAsync(int productId)
        {
            var productComplete = new ProductComplete(productId);

            productComplete.Product = await GetProductAsync(productId);

            if (productComplete.Product == null)
                return productComplete;

            productComplete.ProductType = await GetProductTypeAsync(productComplete.Product.ProductTypeId);

            return productComplete;
        }

        public Task<List<ProductComplete>> GetProductCompletesAsync(List<int> productIds)
        {
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

            return Task.FromResult(result);
        }

        #endregion


        #region Products Public Methods

        public Task<Product?> GetProductAsync(int id)
        {
            var product = products.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(product);
        }

        public Task<List<Product>?> GetProductsAsync()
        {
            return Task.FromResult(products)!;
        }

        #endregion


        #region Product Types Public Methods

        public Task<ProductType?> GetProductTypeAsync(int id)
        {
            var productType = productTypes.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(productType);
        }

        public Task<List<ProductType>?> GetProductTypesAsync()
        {
            return Task.FromResult(productTypes)!;
        }

        #endregion
    }
}
