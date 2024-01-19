using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface IDataApiProxy
    {
        Task<ProductComplete> GetProductCompleteAsync(int productId);
        Task<List<ProductComplete>> GetProductCompletesAsync(List<int> productIds);

        Task<Product?> GetProductAsync(int id);
        Task<List<Product>?> GetProductsAsync();

        Task<ProductType?> GetProductTypeAsync(int id);
        Task<List<ProductType>?> GetProductTypesAsync();
    }
}
