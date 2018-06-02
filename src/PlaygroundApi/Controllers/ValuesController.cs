using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlaygroundApi.Domain.Exceptions;

namespace PlaygroundApi.Controllers
{
    /// <summary>
    /// Values controller. No api version.
    /// </summary>
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;
        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all values.
        /// </summary>
        /// <returns>Collection of values.</returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug($"Get values - ApiVersion: {RequestedApiVersion}");
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Get value by its identifier.
        /// </summary>
        /// <param name="id">Identifier of the value.</param>
        /// <returns>Dummy value.</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (id == 404)
                throw new ResourceNotFoundApiException(ApiErrorCode.ValueNotFound, $"Cannot find value with id={id}");

            return "value";
        }

        /// <summary>
        /// Insert a value.
        /// Not implemented.
        /// </summary>
        /// <param name="value">Value to insert.</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(value)} must be provided.");
        }

        /// <summary>
        /// Update a value.
        /// Not implemented.
        /// </summary>
        /// <param name="id">Identifier of the value to update.</param>
        /// <param name="value">Value to update.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(value)} must be provided.");

            throw new NotImplementedException($"Method {nameof(Put)} not implemented.");
        }

        /// <summary>
        /// Update a value.
        /// Not implemented.
        /// </summary>
        /// <param name="id">Identifier of the value to delete.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            if (id == 42)
                throw new ValidationApiException(ApiErrorCode.DeleteValueForbidden, $"You are not allowed to delete the Answer to the Ultimate Question of Life, the Universe, and Everything.");
        }
    }
}
