using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Playground.Domain;
using Playground.Dto;
using Playground.Services;
using PlaygroundApi.Navigation;

namespace PlaygroundApi.Controllers
{
    /// <summary>
    /// Operations controller.
    /// </summary>
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class OperationsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOperationsService _operationsService;
        public const string RouteNameGetAsync = "Operations_GetAsync";

        /// <summary>
        /// Constructor of operations controller.
        /// </summary>
        /// <param name="operationsService">Operation's service.</param>
        /// <param name="mapper">AutoMapper's mapper.</param>
        public OperationsController(IOperationsService operationsService, IMapper mapper)
        {
            _operationsService = operationsService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get operations list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListDto<OperationDto>), 200)]
        [Route("", Name = RouteNameGetAsync)]
        public async Task<IActionResult> GetAsync(OperationSearchParameter search)
        {
            if (search == null)
                search = new OperationSearchParameter();

            var operations = await _operationsService.GetOperationsAsync(search);
            var pagedListDto = _mapper.Map<PagedListDto<OperationDto>>(operations);
            pagedListDto.BuildNavigationLinks(Request.GetDisplayUrl());
            return Ok(pagedListDto);
        }
    }
}
