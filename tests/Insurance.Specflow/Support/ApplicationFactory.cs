using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.TestHost;
using Insurance.Api;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Specflow.Mock;

namespace Insurance.Specflow.Support
{
    public class ApplicationFactory : WebApplicationFactory<Startup>
    {
        private readonly MockDataApiProxy mockDataApiProxy;

        public ApplicationFactory(MockDataApiProxy mockDataApiProxy)
        {
            this.mockDataApiProxy = mockDataApiProxy;
        }

        public HttpClient GetHttpClient()
        {
            return WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
                {
                    var mockDataApiProxyDescriptor =
                        ServiceDescriptor.Scoped<IDataApiProxy>(serviceProvider => mockDataApiProxy);
                    services.Replace(mockDataApiProxyDescriptor);
                })
            )
            .CreateClient();
        }
    }
}
