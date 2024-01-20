using Microsoft.Extensions.DependencyInjection;
using Insurance.Infrastructure;
using Insurance.Contracts.Application.Interfaces;

namespace Insurance.Application
{
    public static class Startup
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddInfrastructureServices();
            services.AddDatabaseServices();

            services.AddScoped<IInsuranceService, InsuranceService>();
            services.AddScoped<ISurchargeService, SurchargeService>();
        }
    }
}