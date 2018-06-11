using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Playground.Repositories
{
    public interface IProductsRepository
    {
        Task<List<BsonDocument>> GetAllAsync(BsonDocument filters);
        Task<BsonDocument> GetByIdAsync(ObjectId id);
        Task InsertAsync(BsonDocument product);
        Task<long> ReplaceAsync(BsonDocument product);
        Task<long> DeleteAsync(ObjectId id);
    }
}
