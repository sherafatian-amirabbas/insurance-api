using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Database;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurchargeController : Controller
    {
        private readonly ISurchargeService surchargeService;

        public SurchargeController(ISurchargeService surchargeService)
        {
            this.surchargeService = surchargeService;
        }


        [HttpPost]
        public async Task<IActionResult> SaveSurcharge([FromBody] List<Surcharge> Surcharges)
        {
            await surchargeService.SaveSurchargeAsync(Surcharges);
            return Ok();
        }
    }
}
