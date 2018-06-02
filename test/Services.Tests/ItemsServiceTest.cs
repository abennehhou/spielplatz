using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using MongoDB.Bson;
using Moq;
using Playground.Domain;
using Playground.Repositories;
using Services.Tests;
using X.PagedList;
using Xunit;

namespace Playground.Services.Tests
{
    public class ItemsServiceTest
    {
        [Theory, CustomAutoData]
        public async Task GetByIdReturnsExpectedItem(Item expectedItem, ObjectId id,  [Frozen]Mock<IItemsRepository> itemsRepositoryMock, ItemsService itemsService)
        {
            // Arrange
            itemsRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(expectedItem);

            // Act
            var result = await itemsService.GetById(id.ToString());

            // Assert
            Assert.Equal(result, expectedItem);
            itemsRepositoryMock.VerifyAll();
        }

        [Theory, CustomAutoData]
        public async Task InsertItemPerformsOperation(Item item, [Frozen]Mock<IItemsRepository> itemsRepositoryMock, ItemsService itemsService)
        {
            // Arrange
            itemsRepositoryMock.Setup(x => x.InsertItem(item)).Returns(Task.FromResult(0));

            // Act
             await itemsService.InsertItem(item);

            // Assert
            itemsRepositoryMock.VerifyAll();
        }

        [Theory, CustomAutoData]
        public async Task GetItemsReturnsExpectedItems(IPagedList<Item> expectedItems, ItemSearchParameter searchParameters, [Frozen]Mock<IItemsRepository> itemsRepositoryMock, ItemsService itemsService)
        {
            // Arrange
            itemsRepositoryMock.Setup(x => x.GetItems(searchParameters)).ReturnsAsync(expectedItems);

            // Act
            var result = await itemsService.GetItems(searchParameters);

            // Assert
            Assert.Equal(result, expectedItems);
            itemsRepositoryMock.VerifyAll();
        }
    }
}
