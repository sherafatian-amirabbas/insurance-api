using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Insurance.Common.Interfaces;
using System.Net;

namespace Insurance.Common.Services
{
    public class ApplicationHttpClient
    {
        private readonly HttpClient client;
        private readonly IApplicationHttpRequest appHttpRequest;
        private readonly ILogger<ApplicationHttpClient> logger;


        #region Constructors

        public ApplicationHttpClient(HttpClient client,
            IApplicationHttpRequest appHttpRequest,
            ILogger<ApplicationHttpClient> logger)
        {
            this.client = client;
            this.appHttpRequest = appHttpRequest;
            this.logger = logger;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Sends a GET request
        /// </summary>
        public async Task<TResponse?> GetAsync<TResponse>(string path)
            where TResponse : class, new()
        {
            var requestId = appHttpRequest.GetRequestId();
            logger.LogDebug("httpRequest - RequestId: {RequestId}, Path: {Path}",
                requestId,
                path);

            var httpMessage = new HttpRequestMessage(HttpMethod.Get, path);

            var response = await client.SendAsync(httpMessage);
            return await ParseResponse<TResponse>(response);
        }

        #endregion


        #region Private Methods

        public async Task<TResponse?> ParseResponse<TResponse>(HttpResponseMessage? response)
            where TResponse : class, new()
        {
            var requestId = appHttpRequest.GetRequestId();

            if (response == null)
                throw new Exception($"HttpRequest - internal error - RequestId: {requestId}");

            var statusCode = (int)response.StatusCode;
            if (response.IsSuccessStatusCode)
                return await DeserializeResponseAsync<TResponse>(response);

            if (statusCode >= 400 && statusCode < 500) // bad request e.g. not found
                return null;

            throw new Exception($"HttpRequest - internal error - RequestId: {requestId}");
        }

        private async Task<TResponse?> DeserializeResponseAsync<TResponse>(HttpResponseMessage response)
            where TResponse : class, new()
        {
            var content = await response.Content.ReadAsStringAsync();
            var requestId = appHttpRequest.GetRequestId();
            var path = response.RequestMessage?.RequestUri?.LocalPath;
            logger.LogDebug("httpRequest - RequestId: {RequestId}, Path: {Path}, Response: {Response}",
            requestId,
                path,
                content);

            var instance = JsonConvert.DeserializeObject<TResponse>(content);
            return instance;
        }

        #endregion
    }
}