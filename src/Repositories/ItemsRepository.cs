using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Playground.Domain;
using X.PagedList;

namespace Playground.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly PlaygroundContext _playgroundContext;
        private readonly ILogger _logger;

        public ItemsRepository(string connectionString, string databaseName, ILogger<ItemsRepository> logger)
        {
            _playgroundContext = new PlaygroundContext(connectionString, databaseName);
            _logger = logger;
        }

        public async Task<IPagedList<Item>> GetItems(ItemSearchParameter searchParameters)
        {
            var collection = _playgroundContext.GetItemsCollection();

            var filter = Builders<Item>.Filter.Empty;

            if (!string.IsNullOrEmpty(searchParameters.Name))
                filter = filter & Builders<Item>.Filter.Eq(x => x.Name, searchParameters.Name);

            if (!string.IsNullOrEmpty(searchParameters.Owner))
                filter = filter & Builders<Item>.Filter.Eq(x => x.Owner, searchParameters.Owner);

            if (!string.IsNullOrEmpty(searchParameters.Tag))
                filter = filter & Builders<Item>.Filter.AnyEq(x => x.Tags, searchParameters.Tag);

            var query = collection.Find(filter)
                .SortBy(x => x.Id)
                .Skip(searchParameters.Skip)
                .Limit(searchParameters.Limit);

            _logger.LogDebug($"Get items query: {query}.");
            var items = await query.ToListAsync();
            var totalRows = (int)await collection.CountAsync(filter);
            return items.ToPagedList(searchParameters.Skip, searchParameters.Limit, totalRows);
        }

        public async Task<Item> GetById(ObjectId id)
        {
            var collection = _playgroundContext.GetItemsCollection();

            var item = await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return item;
        }

        public async Task InsertItem(Item item)
        {
            await _playgroundContext.GetItemsCollection().InsertOneAsync(item);
        }

        public async Task<long> ReplaceItemAsync(Item item)
        {
            var result = await _playgroundContext.GetItemsCollection().ReplaceOneAsync(x => x.Id == item.Id, item);

            if (result.IsAcknowledged)
                _logger.LogDebug($"Update item '{item.Id}' acknowledged. MatchedCount: {result.MatchedCount}, modifiedCount: {result.ModifiedCount}.");
            else
                _logger.LogDebug($"Update item '{item.Id}' not acknowledged.");

            return result.IsAcknowledged ? result.ModifiedCount : 0;
        }

        public async Task<long> DeleteItemAsync(ObjectId id)
        {
            var result = await _playgroundContext.GetItemsCollection().DeleteOneAsync(x => x.Id == id);

            if (result.IsAcknowledged)
                _logger.LogDebug($"Delete item '{id}' acknowledged. v: {result.DeletedCount}.");
            else
                _logger.LogDebug($"Delete item '{id}' not acknowledged.");

            return result.IsAcknowledged ? result.DeletedCount : 0;
        }
    }
}
