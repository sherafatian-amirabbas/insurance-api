using Microsoft.Extensions.DependencyInjection;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Database;

namespace Insurance.Application
{
    public static class Startup
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            services.AddDbContext<InsuranceDbContext>();
            services.AddScoped<ISurchargeRepository, SurchargeRepository>();
        }
    }
}