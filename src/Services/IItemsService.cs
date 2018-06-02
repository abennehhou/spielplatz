using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.Domain;
using X.PagedList;

namespace Playground.Services
{
    public interface IItemsService
    {
        Task<IPagedList<Item>> GetItems(ItemSearchParameter searchParameter);
        Task<Item> GetById(string id);
        Task InsertItem(Item item);
    }
}
