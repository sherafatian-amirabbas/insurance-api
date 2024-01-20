using Insurance.Contracts.Application.Models;
using Insurance.Contracts.Plugins.Database;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface ISurchargeService
    {
        Task SaveSurchargeAsync(List<Surcharge> surcharges);
        Task<List<Surcharge>> GetSurchargesAsync(List<int> productTypeIds);
    }
}
