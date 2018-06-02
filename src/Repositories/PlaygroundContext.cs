using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Playground.Domain;

namespace Playground.Repositories
{
    public class PlaygroundContext
    {
        private const string CollectionNameItems = "items";
        private const string CollectionNameOperations = "operations";

        private readonly IMongoDatabase _playgroundDatabase;

        public PlaygroundContext(string connectionString, string databaseName)
        {
            RegisterConventions();
            var client = new MongoClient(connectionString);
            _playgroundDatabase = client.GetDatabase(databaseName);
        }

        private void RegisterConventions()
        {
            ConventionRegistry.Register(
                "IgnoreNullValues",
                new ConventionPack
                {
                    new IgnoreIfNullConvention(true)
                },
                t => true);

            ConventionRegistry.Register(
                "CamelCaseElementName",
                new ConventionPack
                {
                    new CamelCaseElementNameConvention()
                },
                t => true);

            ConventionRegistry.Register(
                "EnumAsString",
                new ConventionPack
                {
                    new EnumRepresentationConvention(BsonType.String)
                }, t => true);
            ConventionRegistry.Register(
                "IgnoreExtraElements",
                new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true)
                }, t => true);
        }

        public IMongoCollection<Item> GetItemsCollection()
        {
            return _playgroundDatabase.GetCollection<Item>(CollectionNameItems);
        }

        public IMongoCollection<Operation> GetOperationsCollection()
        {
            return _playgroundDatabase.GetCollection<Operation>(CollectionNameOperations);
        }
    }
}
