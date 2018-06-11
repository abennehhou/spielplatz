using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Playground.Services;
using PlaygroundApi.Domain.Exceptions;

namespace PlaygroundApi.Controllers
{
    [ApiVersionNeutral]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductsService _productsService;
        private readonly IMapper _mapper;
        public const string RouteNameGetById = "Products_GetById";

        public ProductsController(IProductsService productsService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<dynamic>), 200)]
        public async Task<IActionResult> Get()
        {
            var products = await _productsService.GetAllAsync();
            var result = _mapper.Map<List<dynamic>>(products);
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [Route("{id}", Name = RouteNameGetById)]
        public async Task<IActionResult> GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(id)} must be provided.");

            var product = await _productsService.GetByIdAsync(id);

            if (product == null)
                throw new ResourceNotFoundApiException(ApiErrorCode.ProductNotFound, $"Cannot find product with id='{id}'.");

            var result = _mapper.Map<dynamic>(product);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Post([FromBody]dynamic product)
        {
            if (product == null)
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(product)} must be provided.");

            BsonDocument productDocument = _mapper.Map<BsonDocument>(product);

            if (!string.IsNullOrEmpty(productDocument.GetValue("_id", null)?.ToString()))
                throw new ValidationApiException(ApiErrorCode.InvalidInformation, "Parameter _id must be empty during the creation.");

            await _productsService.InsertAsync(productDocument);

            var createdProduct = _mapper.Map<dynamic>(productDocument);

            return CreatedAtRoute(RouteNameGetById, new { id = productDocument.GetValue("_id", null)?.ToString() }, createdProduct);
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [Route("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]dynamic product)
        {
            if (product == null)
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(product)} must be provided.");

            BsonDocument productDocument = _mapper.Map<BsonDocument>(product);

            var productId = productDocument.GetValue("_id", null)?.ToString();

            if (productId != id)
                throw new ValidationApiException(ApiErrorCode.InvalidInformation, $"ProductId must be the same in the uri and in the body. Provided values: '{id}' and '{productId}'.");

            await _productsService.ReplaceAsync(productDocument);

            var updatedProduct = _mapper.Map<dynamic>(productDocument);

            return Ok(updatedProduct);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ValidationApiException(ApiErrorCode.MissingInformation, $"Parameter {nameof(id)} must be provided.");

            await _productsService.DeleteAsync(id);

            return Ok();
        }
    }
}
