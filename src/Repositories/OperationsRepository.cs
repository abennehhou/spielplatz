using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Playground.Domain;
using X.PagedList;

namespace Playground.Repositories
{
    public class OperationsRepository : IOperationsRepository
    {
        private readonly ILogger _logger;
        private readonly PlaygroundContext _playgroundContext;

        public OperationsRepository(string connectionString, string databaseName, ILogger<OperationsRepository> logger)
        {
            _playgroundContext = new PlaygroundContext(connectionString, databaseName);
            _logger = logger;
        }

        public async Task<Operation> GetOperationByIdAsync(ObjectId id)
        {
            var operation = await GetOperationsCollection()
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return operation;
        }

        public async Task<IPagedList<Operation>> GetOperationsAsync(OperationSearchParameter searchParameters)
        {
            if (searchParameters.Limit <= 0)
                return new PagedList<Operation>(new Operation[0], 1, 1);

            var filter = Builders<Operation>.Filter.Empty;

            if (searchParameters.EntityId != null)
                filter = filter & Builders<Operation>.Filter.Eq(x => x.EntityId, searchParameters.EntityId);

            if (searchParameters.EntityType != null)
                filter = filter & Builders<Operation>.Filter.Eq(x => x.EntityType, searchParameters.EntityType);

            var collection = GetOperationsCollection();
            var query = collection.Find(filter)
                .SortBy(x => x.Date)
                .Skip(searchParameters.Skip)
                .Limit(searchParameters.Limit);

            _logger.LogDebug($"Get operations query: {query}.");
            var filteredOperations = await query.ToListAsync();
            var totalRows = (int)await collection.CountAsync(filter);

            return filteredOperations.ToPagedList(searchParameters.Skip, searchParameters.Limit, totalRows);
        }

        public async Task<int> InsertOperationsAsync(IList<Operation> operations)
        {
            if (!operations.Any())
                return 0;

            await GetOperationsCollection().InsertManyAsync(operations, new InsertManyOptions { IsOrdered = true });

            return operations.Where(x => x.Id != ObjectId.Empty).Select(x => x.Id).Distinct().Count();
        }

        public List<Difference> GetDifferences<TEntity>(TEntity document1, TEntity document2) where TEntity : class
        {
            if (document1 == null || document2 == null)
                return new List<Difference>();

            var diff = document1.ToBsonDocument().GetDifferences(document2.ToBsonDocument());
            return diff;
        }

        private IMongoCollection<Operation> GetOperationsCollection()
        {
            return _playgroundContext.GetOperationsCollection();
        }
    }
}
