﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        }

        /// <summary>
        /// Update a value.
        /// Not implemented.
        /// </summary>
        /// <param name="id">Identifier of the value to delete.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
