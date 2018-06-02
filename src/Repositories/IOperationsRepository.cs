using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Domain;
using X.PagedList;

namespace Playground.Repositories
{
    public interface IOperationsRepository
    {
        Task<Operation> GetOperationByIdAsync(ObjectId id);
        Task<IPagedList<Operation>> GetOperationsAsync(OperationSearchParameter searchParameters);
        Task<int> InsertOperationsAsync(IList<Operation> entities);
        List<Difference> GetDifferences<TEntity>(TEntity entity1, TEntity entity2) where TEntity : class;
    }
}
