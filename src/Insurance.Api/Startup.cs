using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Insurance.Api.Middlewares;
using Insurance.Application;
using Insurance.Common.Interfaces;
using Insurance.Contracts.Application.Configuration;
using Insurance.Common.Services;
using Insurance.Common.Services.CircuitBreaker;


namespace Insurance.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AddInsuranceApiServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            // swagger
            app.UseSwagger();
            app.UseSwaggerUI();

            LoggerFactory.Create(builder => builder.AddConsole()); // to log in the console
            app.UseApplicationExceptionHandler(); // exception handling middleware to map the exceptions to response

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        #region Private Methods

        /// <summary>
        /// Registers Application Services
        /// </summary>
        private void AddInsuranceApiServices(IServiceCollection services)
        {
            // swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            AddSettings(services);

            services.AddHttpClient<ApplicationHttpClient>((serviceProvider, httpClient) =>
            {
                var dataApiSetting = serviceProvider.GetRequiredService<IOptions<DataApiSetting>>().Value;
                httpClient.BaseAddress = new Uri(dataApiSetting.BaseUrl);
            });

            services.AddScoped<IApplicationHttpRequest, ApplicationHttpRequest>();
            services.AddSingleton<ICircuitBreakerFactory, CircuitBreakerFactory>();
            services.AddMemoryCache();

            services.AddApplicationServices();
        }

        /// <summary>
        /// Registers Configurations
        /// </summary>
        private void AddSettings(IServiceCollection services)
        {
            services.Configure<DataApiSetting>(Configuration.GetSection("DataApi"));
        }

        #endregion
    }
}
