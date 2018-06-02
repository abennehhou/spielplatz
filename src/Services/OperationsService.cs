using System.Threading.Tasks;
using Playground.Domain;
using Playground.Repositories;
using X.PagedList;

namespace Playground.Services
{
    public class OperationsService : IOperationsService
    {
        #region Private fields

        private readonly IOperationsRepository _operationsRepository;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsService" /> class.
        /// </summary>
        /// <param name="operationRepository">Operation repository. </param>
        public OperationsService(IOperationsRepository operationRepository)
        {
            _operationsRepository = operationRepository;
        }

        #endregion

        #region Public Methods

        public async Task<IPagedList<Operation>> GetOperationsAsync(OperationSearchParameter searchParameters)
        {
            var result = await _operationsRepository.GetOperationsAsync(searchParameters);
            return result;
        }

        #endregion
    }
}
