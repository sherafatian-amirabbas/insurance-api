namespace Insurance.Contracts.Application.Models
{
    public record OrderInsuranceResult(List<int> ProductIds, float InsuranceValue);
}
