using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Playground.Domain;

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

        public async Task<List<Item>> GetAllItems()
        {
            var collection = _playgroundContext.GetItemsCollection();
            var query = collection.Find(x => true);

            _logger.LogDebug($"Get items query: {query}.");
            var items = await query.ToListAsync();

            return items;
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
    }
}
