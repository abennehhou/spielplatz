using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Playground.Domain;

namespace Playground.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        private static ConcurrentBag<Item> Items = new ConcurrentBag<Item>
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

        public List<Item> GetAllItems()
        {
            return Items.ToList();
        }

        public Item GetById(string id)
        {
            return Items.FirstOrDefault(x => x.Id == id);
        }

        public void InsertItem(Item item)
        {
            item.Id = Guid.NewGuid().ToString();
            Items.Add(item);
        }
    }
}
