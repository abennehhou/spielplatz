using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Repositories;
using PlaygroundApi.Domain.Exceptions;

namespace Playground.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;

        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<List<BsonDocument>> GetAllAsync()
        {
            return await _productsRepository.GetAll();
        }

        public async Task<BsonDocument> GetByIdAsync(string id)
        {
            var objectId = ObjectId.Empty;
            ObjectId.TryParse(id, out objectId);

            return await _productsRepository.GetById(objectId);
        }

        public async Task InsertAsync(BsonDocument product)
        {
            await _productsRepository.Insert(product);
        }

        public async Task<long> ReplaceAsync(BsonDocument product)
        {
            if (product == null)
                throw new ApiException(ApiErrorCode.MissingInformation, $"Missing parameter: ${nameof(product)}");

            var retrievedProduct = await GetByIdAsync(product.GetValue("_id", null)?.ToString());

            if (retrievedProduct == null)
                throw new ValidationApiException(ApiErrorCode.ProductNotFound, $"Cannot find product with id={product["_id"]}");

            var result = await _productsRepository.ReplaceAsync(product);

            return result;
        }

        public async Task<long> DeleteAsync(string id)
        {
            var retrievedProduct = await GetByIdAsync(id);

            if (retrievedProduct == null)
                throw new ValidationApiException(ApiErrorCode.ProductNotFound, $"Cannot find product with id={id}");

            var objectId = ObjectId.Empty;
            ObjectId.TryParse(id, out objectId);

            var result = await _productsRepository.DeleteAsync(objectId);

            return result;
        }
    }
}
