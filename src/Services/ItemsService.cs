using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Domain;
using Playground.Repositories;
using X.PagedList;

namespace Playground.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public async Task<IPagedList<Item>> GetItems(ItemSearchParameter searchParameter)
        {
            return await _itemsRepository.GetItems(searchParameter);
        }

        public async Task<Item> GetById(string id)
        {
            var objectId = ObjectId.Empty;
            ObjectId.TryParse(id, out objectId);

            return await _itemsRepository.GetById(objectId);
        }

        public async Task InsertItem(Item item)
        {
            await _itemsRepository.InsertItem(item);
        }
    }
}
