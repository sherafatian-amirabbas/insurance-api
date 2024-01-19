using System.Threading.Tasks;
using Insurance.Api.Models;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    public class HomeController: Controller
    {
        private readonly IInsuranceService insuranceService;

        public HomeController(IInsuranceService insuranceService)
        {
            this.insuranceService = insuranceService;
        }

        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<ProductInsurance> CalculateInsurance([FromBody] CalculateInsuranceModel product)
        {
            return await insuranceService.CalculateInsuranceAsync(product.ProductId);
        }
    }
}