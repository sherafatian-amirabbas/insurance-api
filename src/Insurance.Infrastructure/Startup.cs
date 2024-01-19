using Insurance.Contracts.Application.Interfaces;
using Insurance.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Insurance.Infrastructure
{
    public static class Startup
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDataApiProxy, DataApiProxy>();
        }
    }
}