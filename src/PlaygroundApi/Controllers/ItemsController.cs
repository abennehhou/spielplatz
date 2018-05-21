using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Playground.Domain;
using Playground.Dto;
using Playground.Services;
using PlaygroundApi.Exceptions;
using PlaygroundApi.Validation;

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
        private const string RouteNameGetById = "Items_GetById";

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
        /// <returns>Collection of items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ItemDto>), 200)]
        public IActionResult Get()
        {
            _logger.LogDebug($"Get items - ApiVersion: {RequestedApiVersion}");
            var items = _itemsService.GetAllItems();
            var itemDtos = _mapper.Map<List<ItemDto>>(items);
            return Ok(itemDtos);
        }

        /// <summary>
        /// Get an item by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ItemDto), 200)]
        [Route("{id}", Name = RouteNameGetById)]
        public IActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(id)} must be provided.");

            var item = _itemsService.GetById(id);

            if (item == null)
                throw new ResourceNotFoundApiException(ApiErrorCode.ItemNotFound, $"Cannot find item with id='{id}'.");

            var itemDto = _mapper.Map<ItemDto>(item);

            return Ok(itemDto);
        }

        /// <summary>
        /// Insert an item.
        /// </summary>
        /// <param name="itemDto">Item to insert.</param>
        [HttpPost]
        [ValidateCommand]
        [ProducesResponseType(typeof(ItemDto), 201)]
        public IActionResult Post([FromBody]ItemDto itemDto)
        {
            if (itemDto == null)
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(itemDto)} must be provided.");

            if (!string.IsNullOrEmpty(itemDto.Id))
                throw new ValidationApiException(ApiErrorCode.InvalidInformation, $"Parameter {nameof(itemDto.Id)} must be empty during the creation.");

            var item = _mapper.Map<Item>(itemDto);
            _itemsService.InsertItem(item);

            var createdItemDto = _mapper.Map<ItemDto>(item);

            return CreatedAtRoute(RouteNameGetById, new { id = item.Id }, createdItemDto);
        }
    }
}
