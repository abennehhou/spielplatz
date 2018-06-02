using MongoDB.Bson;

namespace Playground.Domain
{
    public class Difference
    {
        public string PropertyName { get; set; }

        public BsonValue Value1 { get; set; }

        public BsonValue Value2 { get; set; }
    }
}
