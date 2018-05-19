using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PlaygroundApi.Exceptions;

namespace PlaygroundApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            _logger.LogWarning(exception.Message);
            _logger.LogTrace($"Stacktrace: {exception.StackTrace}");
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
                _logger.LogWarning($"Inner exception: {exception.Message}");
                _logger.LogTrace($"Stacktrace: {exception.StackTrace}");
            }

            var apiException = exception as ApiException;
            if (apiException != null)
                code = GetHttpStatusCodeFromException(apiException);

            var apiErrorCode = apiException?.ApiErrorCode ?? ApiErrorCode.InternalError;

            var result = JsonConvert.SerializeObject(new { Error = exception.Message, ApiErrorCode = apiErrorCode.ToString() });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

        private HttpStatusCode GetHttpStatusCodeFromException(ApiException exception)
        {
            if (exception is ResourceNotFoundApiException)
                return HttpStatusCode.NotFound;

            if (exception is ValidationApiException)
                return HttpStatusCode.BadRequest;

            return HttpStatusCode.InternalServerError;
        }
    }
}
