using System.Collections.Generic;
using Playground.Domain;

namespace Playground.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        public List<Item> GetAllItems()
        {
            return new List<Item>
            {
                new Item
                {
                    Id = "1",
                    Name = "My book",
                    Owner = "me",
                    Description = "my description"
                },
                new Item
                {
                    Id = "2",
                    Name = "Your computer",
                    Owner = "you",
                    Description = "your description"
                }
            };
        }
    }
}
