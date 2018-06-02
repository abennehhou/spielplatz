using System.Threading.Tasks;
using Playground.Domain;
using X.PagedList;

namespace Playground.Services
{
    public interface IOperationsService
    {
        Task<IPagedList<Operation>> GetOperationsAsync(OperationSearchParameter searchParameters);
    }
}
