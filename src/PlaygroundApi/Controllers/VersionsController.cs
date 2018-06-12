using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Playground.Dto;

namespace PlaygroundApi.Controllers
{
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class VersionsController : Controller
    {
        private readonly ILogger<VersionsController> _logger;
        private readonly IMapper _mapper;
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public VersionsController(ILogger<VersionsController> logger, IMapper mapper, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(VersionDto), 200)]
        public IActionResult Get()
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var apiDescriptions = _apiVersionDescriptionProvider.ApiVersionDescriptions;
            var versionDto = new VersionDto
            {
                AssemblyVersion = assemblyVersion,
                SupportedApiVersions = _mapper.Map<List<VersionDescriptionDto>>(apiDescriptions)
            };

            return Ok(versionDto);
        }
    }
}
