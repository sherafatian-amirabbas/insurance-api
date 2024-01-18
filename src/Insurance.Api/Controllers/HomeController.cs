using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
        [HttpPost]
        [Route("api/insurance/product")]
        public InsuranceDto CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            int productId = toInsure.ProductId;

            BusinessRules.GetProductType(ProductApi, productId, ref toInsure);

            if (!toInsure.ProductTypeHasInsurance)
            {
                toInsure.InsuranceValue = 0;
                return toInsure;
            }

            BusinessRules.GetSalesPrice(ProductApi, productId, ref toInsure);

            float insurance = 0f;
            if (toInsure.SalesPrice >= 500)
                insurance = toInsure.SalesPrice < 2000 ? 1000 : 2000;

            if (toInsure.ProductTypeName == "Laptops" || toInsure.ProductTypeName == "Smartphones")
                insurance += 500;

            toInsure.InsuranceValue = insurance;
            return toInsure;
        }

        public class InsuranceDto
        {
            public int ProductId { get; set; }
            public float InsuranceValue { get; set; }
            [JsonIgnore]
            public string ProductTypeName { get; set; }
            [JsonIgnore]
            public bool ProductTypeHasInsurance { get; set; }
            [JsonIgnore]
            public float SalesPrice { get; set; }
        }

        private const string ProductApi = "http://localhost:5002";
    }
}