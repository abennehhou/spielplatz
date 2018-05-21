using System.Collections.Generic;
using Playground.Domain;
using Playground.Repositories;

namespace Playground.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsService(IItemsRepository itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        public List<Item> GetAllItems()
        {
            return _itemsRepository.GetAllItems();
        }

        public Item GetById(string id)
        {
            return _itemsRepository.GetById(id);
        }

        public void InsertItem(Item item)
        {
            _itemsRepository.InsertItem(item);
        }
    }
}
