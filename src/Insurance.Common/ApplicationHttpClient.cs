﻿using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Insurance.Common.Interfaces;

namespace Insurance.Common
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
        public async Task<TResponse?> GetAsync<TResponse>(string path,
            HttpContent? content = null)
            where TResponse : class, new()
        {
            var requestId = appHttpRequest.GetRequestId();
            logger.LogInformation("httpRequest - RequestId: {RequestId}, Path: {Path}",
                requestId,
                path);

            var httpMessage = new HttpRequestMessage(HttpMethod.Get, path);

            if (content != null)
                httpMessage.Content = content;

            var response = await client.SendAsync(httpMessage);
            return await DeserializeResponseAsync<TResponse>(response);
        }

        #endregion


        #region Private Methods

        private async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage? response)
            where T : class, new()
        {
            if (response == null)
                return null;

            var content = await response.Content.ReadAsStringAsync();

            var requestId = appHttpRequest.GetRequestId();
            var path = response.RequestMessage?.RequestUri?.LocalPath;
            logger.LogInformation("httpRequest - RequestId: {RequestId}, Path: {Path}, Response: {Response}",
            requestId,
                path,
                content);

            var instance = JsonConvert.DeserializeObject<T>(content);
            return instance;
        }

        #endregion
    }
}