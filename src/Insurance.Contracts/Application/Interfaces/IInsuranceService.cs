using Insurance.Contracts.Application.Models;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface IInsuranceService
    {
        Task<ProductInsurance> CalculateInsuranceAsync(int productId);
    }
}
