using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Playground.Repositories
{
    public interface IProductsRepository
    {
        Task<List<BsonDocument>> GetAll();
        Task<BsonDocument> GetById(ObjectId id);
        Task Insert(BsonDocument product);
        Task<long> ReplaceAsync(BsonDocument product);
        Task<long> DeleteAsync(ObjectId id);
    }
}
