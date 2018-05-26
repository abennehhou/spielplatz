using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Domain;

namespace Playground.Services
{
    public interface IItemsService
    {
        Task<List<Item>> GetAllItems();
        Task<Item> GetById(string id);
        Task InsertItem(Item item);
    }
}
