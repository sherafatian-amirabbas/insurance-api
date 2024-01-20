namespace Insurance.Contracts.Application.Models
{
    public class OrderInsuranceResult
    {
        #region Constructors

        public OrderInsuranceResult()
        { }

        public OrderInsuranceResult(List<int> productIds, float insuranceValue)
        {
            ProductIds = productIds;
            InsuranceValue = insuranceValue;
        }

        #endregion


        public List<int> ProductIds { get; } = new List<int>();
        public float InsuranceValue { get; set; }
    }
}
