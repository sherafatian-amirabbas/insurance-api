using Insurance.Contracts.Plugins.Database;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface ISurchargeRepository
    {
        Task SaveSurchargeAsync(List<Surcharge> surcharges);
        Task<List<Surcharge>> GetSurchargesAsync(List<int> productTypeIds);
    }
}
