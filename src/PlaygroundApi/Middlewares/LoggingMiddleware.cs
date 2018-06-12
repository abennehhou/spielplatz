using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace PlaygroundApi.Middlewares
{
    public class LoggingMiddleware
    {
        private const string SwaggerUrl = "/swagger";
        private const string VersionsUrl = "/versions";
        private static readonly string[] UrlsToIgnore = { SwaggerUrl, VersionsUrl };

        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var request = context.Request;
            var requestUri = request.GetDisplayUrl();
            if (UrlsToIgnore.Any(x => requestUri.ToLower().Contains(x))) // Ignore swagger documentation page and version calls
            {
                await _next(context);
                return;
            }

            var stopwatch = Stopwatch.StartNew();
            string statusCode = null;

            try
            {
                await _next(context);
                var response = context.Response;
                statusCode = response.StatusCode.ToString();
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogDebug($"RequestMethod={request.Method};RequestUri={requestUri};ResponseCode={statusCode};ElapsedTime={stopwatch.Elapsed}");
            }
        }
    }
}
