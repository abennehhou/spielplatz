using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PlaygroundApi.Controllers
{
    /// <summary>
    /// Controller used to test versioning with query parameters.
    /// </summary>
    [ApiVersion("1.0")]
    [ApiVersion("1.0-alpha1", Deprecated = true)]
    [ApiVersion("1.0-alpha2")]
    [Route("api/[controller]")]
    public class VersioningTestController : Controller
    {
        private readonly ILogger<VersioningTestController> _logger;
        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public VersioningTestController(ILogger<VersioningTestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all values version 1.0.
        /// </summary>
        /// <returns>Collection of values.</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get all values version 1.0-alpha1.
        /// </summary>
        /// <returns>Collection of values.</returns>
        [HttpGet]
        [MapToApiVersion("1.0-alpha1")]
        public IEnumerable<string> GetAlpha1()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1-alpha1", "value2-alpha1" };
        }

        /// <summary>
        /// Get all values version 1.0-alpha2.
        /// </summary>
        /// <returns>Collection of values.</returns>
        [HttpGet]
        [MapToApiVersion("1.0-alpha2")]
        public IEnumerable<string> GetAlpha2()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1-alpha2", "value2-alpha2" };
        }

        /// <summary>
        /// Get value by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the value.</param>
        /// <returns>Dummy value.</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _logger.LogDebug($"Get by id {id} - ApiVersion: {RequestedApiVersion}");
            return "value";
        }
    }
}
