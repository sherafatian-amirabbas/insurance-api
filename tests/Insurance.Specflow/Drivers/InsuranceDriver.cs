using System.Net;
using System.Text;
using Newtonsoft.Json;
using Insurance.Api.Models;
using Insurance.Common.Constants;
using Insurance.Contracts.Application.Models;
using Insurance.Specflow.Support;

namespace Insurance.SpecFlow.Drivers
{
    public class InsuranceDriver
    {
        private const string CALCULATE_PRODUCT_INSURANCE_ENDPOINT = "/api/insurance/product";
        private const string CALCULATE_ORDER_INSURANCE_ENDPOINT = "/api/insurance/order";

        private readonly HttpClient client;


        public InsuranceDriver(ApplicationFactory appFactory)
        {
            this.client = appFactory.GetHttpClient();
        }


        #region Public Methods

        public async Task<ProductInsuranceResult?> CalculateProductInsuranceAsync(int productId)
        {
            var result = await PostAsync<ProductInsuranceResult>(CALCULATE_PRODUCT_INSURANCE_ENDPOINT, 
                new ProductInsurance()
                {
                    ProductId = productId
                }
            );

            return result;
        }

        public async Task<OrderInsuranceResult?> CalculateOrderInsuranceAsync(List<int> productIds)
        {
            var result = await PostAsync<OrderInsuranceResult>(CALCULATE_ORDER_INSURANCE_ENDPOINT,
                new OrderInsurance()
                {
                    ProductIds = productIds
                }
            );

            return result;
        }

        #endregion


        #region Private Methods

        private async Task<TResponse?> PostAsync<TResponse>(string path, object content)
            where TResponse : class, new()
        {
            var httpMessage = new HttpRequestMessage(HttpMethod.Post, path);

            var stringifiedContent = JsonConvert.SerializeObject(content);
            httpMessage.Content = new StringContent(stringifiedContent,
                Encoding.UTF8,
                ContentTypes.CONTENT_TYPE_JSON);

            var response = await client.SendAsync(httpMessage);
            var result = await DeserializeResponseAsync<TResponse>(response);
            return result;
        }

        private async Task<TResponse?> DeserializeResponseAsync<TResponse>(HttpResponseMessage? response)
            where TResponse : class, new()
        {
            if (response == null || response.StatusCode != HttpStatusCode.OK)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResponse>(content);
            return result;
        }

        #endregion
    }
}
