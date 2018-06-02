using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Playground.Domain;
using Playground.Dto;
using Playground.Services;
using PlaygroundApi.Exceptions;
using PlaygroundApi.Navigation;
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
        /// Get all items filtered with search parameters.
        /// </summary>
        /// <returns>Collection of items.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PagedListDto<ItemDto>), 200)]
        public async Task<IActionResult> Get(ItemSearchParameter search)
        {
            _logger.LogDebug($"Get items - ApiVersion: {RequestedApiVersion}");
            var items = await _itemsService.GetItems(search);
            var itemDtos = _mapper.Map<PagedListDto<ItemDto>>(items);
            itemDtos.BuildNavigationLinks(Request.GetDisplayUrl());
            return Ok(itemDtos);
        }

        /// <summary>
        /// Get an item by its identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ItemDto), 200)]
        [Route("{id}", Name = RouteNameGetById)]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(id)} must be provided.");

            var item = await _itemsService.GetById(id);

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
        public async Task<IActionResult> Post([FromBody]ItemDto itemDto)
        {
            if (itemDto == null)
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(itemDto)} must be provided.");

            if (!string.IsNullOrEmpty(itemDto.Id))
                throw new ValidationApiException(ApiErrorCode.InvalidInformation, $"Parameter {nameof(itemDto.Id)} must be empty during the creation.");

            var item = _mapper.Map<Item>(itemDto);
            await _itemsService.InsertItem(item);

            var createdItemDto = _mapper.Map<ItemDto>(item);

            return CreatedAtRoute(RouteNameGetById, new { id = item.Id }, createdItemDto);
        }
    }
}
