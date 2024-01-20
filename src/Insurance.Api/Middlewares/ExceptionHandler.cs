using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Insurance.Common.Interfaces;
using Insurance.Contracts.Application.Exceptions;
using Insurance.Common.Services.CircuitBreaker;
using Insurance.Common.Constants;

namespace Insurance.Api.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate next;


        public ExceptionHandler(RequestDelegate next)
        {
            this.next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext,
            IApplicationHttpRequest appHttpRequest,
            ILogger<ExceptionHandler> logger)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                var requestId = appHttpRequest.GetRequestId();


                // logging the exception with message - in case of technical exception, the actual message
                // is mapped to something general in the response, but the concrete one is logged together
                // with the RequestId
                logger.LogError("Exception - RequestId: {RequestId}, Message: {Message}",
                    requestId,
                    ex.Message);


                httpContext.Response.Clear();

                httpContext.Response.ContentType = ContentTypes.CONTENT_TYPE_JSON;
                httpContext.Response.StatusCode = (int)GetStatusCode(ex);
                var msg = GetMessage(ex);

                var content = JsonConvert.SerializeObject(new
                {
                    // only BusinessException messages are written to the response
                    Message = msg, //ex is BusinessException ? ex.Message : "internal server error",
                    RequestId = requestId
                });

                await httpContext.Response.WriteAsync(content);
            }
        }


        #region Private Methods

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            return ex switch
            {
                AppException => (ex as AppException).StatusCode,
                CircuitBreakerOpenException => HttpStatusCode.ServiceUnavailable,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private string GetMessage(Exception ex)
        {
            return ex switch
            {
                BusinessException => (ex as BusinessException).Message,
                CircuitBreakerOpenException => "Service is currently not available!",
                _ => "internal server error"
            };
        }

        #endregion
    }

    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseApplicationExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandler>();
        }
    }
}
