using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Domain;

namespace Playground.Repositories
{
    public interface IItemsRepository
    {
        Task<List<Item>> GetAllItems();
        Task<Item> GetById(ObjectId id);
        Task InsertItem(Item item);
    }
}
