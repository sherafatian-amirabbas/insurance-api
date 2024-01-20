namespace Insurance.Contracts.Application.Models
{
    public class ProductInsuranceResult
    {
        #region Constructors

        public ProductInsuranceResult()
        { }

        public ProductInsuranceResult(int productId, float insuranceValue)
        {
            ProductId = productId;
            InsuranceValue = insuranceValue;
        }

        #endregion


        public int ProductId { get; set;  }
        public float InsuranceValue { get; set; }
    }
}
