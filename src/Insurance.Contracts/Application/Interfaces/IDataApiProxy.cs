using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface IDataApiProxy
    {
        Task<Product?> GetProductAsync(int id);
        Task<ProductType?> GetProductTypeAsync(int id);
    }
}
