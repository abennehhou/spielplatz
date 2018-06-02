using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Domain;
using X.PagedList;

namespace Playground.Repositories
{
    public interface IItemsRepository
    {
        Task<IPagedList<Item>> GetItems(ItemSearchParameter searchParameter);
        Task<Item> GetById(ObjectId id);
        Task InsertItem(Item item);
        Task<long> ReplaceItemAsync(Item item);
        Task<long> DeleteItemAsync(ObjectId id);
    }
}
