using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Playground.Domain
{
    public class Item
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
    }
}
