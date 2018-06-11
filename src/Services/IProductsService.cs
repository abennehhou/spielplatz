using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Playground.Services
{
    public interface IProductsService
    {
        Task<List<BsonDocument>> GetAllAsync();
        Task<BsonDocument> GetByIdAsync(string id);
        Task InsertAsync(BsonDocument product);
        Task<long> ReplaceAsync(BsonDocument product);
        Task<long> DeleteAsync(string id);
    }
}
