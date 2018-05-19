using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PlaygroundApi.Controllers
{
    /// <summary>
    /// Controller used to test versioning with url path segment.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/VersioningTest2")]
    public class VersioningTest2Controller : Controller
    {
        private readonly ILogger<VersioningTest2Controller> _logger;
        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public VersioningTest2Controller(ILogger<VersioningTest2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1", "value2" };
        }
    }

    [ApiVersion("1.0-alpha1", Deprecated = true)]
    [Route("api/v{version:apiVersion}/VersioningTest2")]
    public class VersioningTest2Alpha1Controller : Controller
    {
        private readonly ILogger<VersioningTest2Alpha1Controller> _logger;
        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public VersioningTest2Alpha1Controller(ILogger<VersioningTest2Alpha1Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1-alpha1", "value2-alpha1" };
        }
    }

    [ApiVersion("1.0-alpha2")]
    [Route("api/v{version:apiVersion}/VersioningTest2")]
    public class VersioningTest2Alpha2Controller : Controller
    {
        private readonly ILogger<VersioningTest2Alpha2Controller> _logger;
        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public VersioningTest2Alpha2Controller(ILogger<VersioningTest2Alpha2Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1-alpha2", "value2-alpha2" };
        }
    }
}
