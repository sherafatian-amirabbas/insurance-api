using System.Threading.Tasks;
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
        public async Task<ProductInsuranceResult> CalculateProductInsurance([FromBody] ProductInsurance productInsurance)
        {
            return await insuranceService.CalculateProductInsuranceAsync(productInsurance);
        }

        [HttpPost]
        [Route("api/insurance/order")]
        public async Task<OrderInsuranceResult> CalculateOrderInsurance([FromBody] OrderInsurance orderInsurance)
        {
            return await insuranceService.CalculateOrderInsuranceAsync(orderInsurance);
        }
    }
}