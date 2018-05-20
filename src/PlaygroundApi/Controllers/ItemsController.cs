using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Playground.Dto;
using Playground.Services;

namespace PlaygroundApi.Controllers
{
    /// <summary>
    /// Items controller.
    /// </summary>
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly ILogger<ItemsController> _logger;
        private readonly IItemsService _itemsService;
        private readonly IMapper _mapper;

        private ApiVersion RequestedApiVersion => HttpContext.ApiVersionProperties()?.ApiVersion;

        public ItemsController(IItemsService itemsService, IMapper mapper, ILogger<ItemsController> logger)
        {
            _itemsService = itemsService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all items.
        /// </summary>
        /// <returns>Collection of values.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ItemDto>), 200)]
        public List<ItemDto> Get()
        {
            _logger.LogDebug($"Get items - ApiVersion: {RequestedApiVersion}");
            var items = _itemsService.GetAllItems();
            return _mapper.Map<List<ItemDto>>(items);
        }
    }
}
