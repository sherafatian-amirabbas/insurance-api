using Insurance.Contracts.Application.Models;

namespace Insurance.Contracts.Application.Interfaces
{
    public interface IInsuranceService
    {
        Task<ProductInsuranceResult> CalculateProductInsuranceAsync(ProductInsurance productInsurance);
        Task<OrderInsuranceResult> CalculateOrderInsuranceAsync(OrderInsurance orderInsurance);
    }
}
