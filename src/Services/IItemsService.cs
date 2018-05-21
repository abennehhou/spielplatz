using System.Collections.Generic;
using Playground.Domain;

namespace Playground.Services
{
    public interface IItemsService
    {
        List<Item> GetAllItems();
        Item GetById(string id);
        void InsertItem(Item item);
    }
}
