using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using X.PagedList;

namespace Playground.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly PlaygroundContext _playgroundContext;
        private readonly ILogger _logger;

        public ProductsRepository(string connectionString, string databaseName, ILogger<ProductsRepository> logger)
        {
            _playgroundContext = new PlaygroundContext(connectionString, databaseName);
            _logger = logger;
        }

        public async Task<List<BsonDocument>> GetAll()
        {
            var collection = _playgroundContext.GetProductsCollection();

            var query = collection.Find(x => true);

            _logger.LogDebug($"Get products query: {query}.");
            var products = await query.ToListAsync();
            return products;
        }

        public async Task<BsonDocument> GetById(ObjectId id)
        {
            var collection = _playgroundContext.GetProductsCollection();

            var product = await collection.Find(x => x["_id"] == id).FirstOrDefaultAsync();

            return product;
        }

        public async Task Insert(BsonDocument product)
        {
            await _playgroundContext.GetProductsCollection().InsertOneAsync(product);
        }

        public async Task<long> ReplaceAsync(BsonDocument product)
        {
            var id = product.GetValue("_id", null);
            var result = await _playgroundContext.GetProductsCollection().ReplaceOneAsync(x => x["_id"] == id, product);

            if (result.IsAcknowledged)
                _logger.LogDebug($"Update product '{id}' acknowledged. MatchedCount: {result.MatchedCount}, modifiedCount: {result.ModifiedCount}.");
            else
                _logger.LogDebug($"Update product '{id}' not acknowledged.");

            return result.IsAcknowledged ? result.ModifiedCount : 0;
        }

        public async Task<long> DeleteAsync(ObjectId id)
        {
            var result = await _playgroundContext.GetProductsCollection().DeleteOneAsync(x => x["_id"] == id);

            if (result.IsAcknowledged)
                _logger.LogDebug($"Delete product '{id}' acknowledged. v: {result.DeletedCount}.");
            else
                _logger.LogDebug($"Delete product '{id}' not acknowledged.");

            return result.IsAcknowledged ? result.DeletedCount : 0;
        }
    }
}
