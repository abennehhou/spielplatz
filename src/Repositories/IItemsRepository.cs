using System.Collections.Generic;
using Playground.Domain;

namespace Playground.Repositories
{
    public interface IItemsRepository
    {
        List<Item> GetAllItems();
    }
}
