using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using MongoDB.Bson;
using Moq;
using Playground.Domain;
using Playground.Repositories;
using Services.Tests;
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
        public async Task GetAllItemsReturnsExpectedItems(List<Item> expectedItems, [Frozen]Mock<IItemsRepository> itemsRepositoryMock, ItemsService itemsService)
        {
            // Arrange
            itemsRepositoryMock.Setup(x => x.GetAllItems()).ReturnsAsync(expectedItems);

            // Act
            var result = await itemsService.GetAllItems();

            // Assert
            Assert.Equal(result, expectedItems);
            itemsRepositoryMock.VerifyAll();
        }
    }
}
