using System.Collections.Generic;
using Playground.Domain;

namespace Playground.Services
{
    public interface IItemsService
    {
        List<Item> GetAllItems();
    }
}
