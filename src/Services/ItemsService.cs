using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Playground.Domain;
using Playground.Repositories;
using PlaygroundApi.Domain.Exceptions;
using X.PagedList;

namespace Playground.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepository _itemsRepository;
        private readonly IOperationsRepository _operationsRepository;
        private const string EntityType = "item";

        public ItemsService(IItemsRepository itemsRepository, IOperationsRepository operationsRepository)
        {
            _itemsRepository = itemsRepository;
            _operationsRepository = operationsRepository;
        }

        public async Task<IPagedList<Item>> GetItems(ItemSearchParameter searchParameter)
        {
            return await _itemsRepository.GetItems(searchParameter);
        }

        public async Task<Item> GetById(string id)
        {
            var objectId = ObjectId.Empty;
            ObjectId.TryParse(id, out objectId);

            return await _itemsRepository.GetById(objectId);
        }

        public async Task InsertItem(Item item)
        {
            await _itemsRepository.InsertItem(item);

            var operation = new Operation
            {
                Date = DateTime.UtcNow,
                EntityId = item.Id.ToString(),
                OperationType = OperationTypeEnum.Create.ToString(),
                EntityType = EntityType
            };
            await _operationsRepository.InsertOperationsAsync(new[] { operation });
        }

        public async Task<long> ReplaceItemAsync(Item item)
        {
            if (item == null)
                throw new ApiException(ApiErrorCode.MissingInformation, $"Missing parameter: ${nameof(item)}");

            var retrievedItem = await GetById(item.Id.ToString());

            if (retrievedItem == null)
                throw new ValidationApiException(ApiErrorCode.ItemNotFound, $"Cannot find item with id={item.Id}");

            var result = await _itemsRepository.ReplaceItemAsync(item);

            var differences = _operationsRepository.GetDifferences(retrievedItem, item);
            var operation = new Operation
            {
                Date = DateTime.UtcNow,
                EntityId = item.Id.ToString(),
                OperationType = OperationTypeEnum.Update.ToString(),
                Differences = differences,
                EntityType = EntityType
            };
            await _operationsRepository.InsertOperationsAsync(new[] { operation });

            return result;
        }

        public async Task<long> DeleteItemAsync(string id)
        {
            var retrievedItem = await GetById(id);

            if (retrievedItem == null)
                throw new ValidationApiException(ApiErrorCode.ItemNotFound, $"Cannot find item with id={id}");

            var objectId = ObjectId.Empty;
            ObjectId.TryParse(id, out objectId);

            var result = await _itemsRepository.DeleteItemAsync(objectId);

            var operation = new Operation
            {
                Date = DateTime.UtcNow,
                EntityId = id,
                OperationType = OperationTypeEnum.Delete.ToString(),
                EntityType = EntityType
            };

            await _operationsRepository.InsertOperationsAsync(new[] { operation });

            return result;
        }
    }
}
