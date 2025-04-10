using System;
using System.Net;
using System.Threading.Tasks;
using Coterie.Api.Models.Domain.Responses;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Coterie.Api.ExceptionHelpers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var response = context.Response;
            var appContext = context.Features.Get<IExceptionHandlerPathFeature>();

            BaseExceptionResponse ex = default;

            switch (appContext.Error)
            {
                case IndexOutOfRangeException:
                case NullReferenceException:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case ArgumentException:
                    // Bad Request status
                    response.StatusCode = (int) HttpStatusCode.BadRequest;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }
            _logger.LogError(appContext.Error, "Unhandled exception occurred");
            ex = new BaseExceptionResponse
            {
                Message = appContext.Error.Message
            };

            await response.WriteAsJsonAsync(ex);
        }
    }
}