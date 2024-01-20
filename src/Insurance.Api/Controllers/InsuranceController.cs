using System.Threading.Tasks;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController: Controller
    {
        private readonly IInsuranceService insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            this.insuranceService = insuranceService;
        }

        [HttpPost]
        [Route("product")]
        public async Task<ProductInsuranceResult> CalculateProductInsurance([FromBody] ProductInsurance productInsurance)
        {
            return await insuranceService.CalculateProductInsuranceAsync(productInsurance);
        }

        [HttpPost]
        [Route("order")]
        public async Task<OrderInsuranceResult> CalculateOrderInsurance([FromBody] OrderInsurance orderInsurance)
        {
            return await insuranceService.CalculateOrderInsuranceAsync(orderInsurance);
        }
    }
}