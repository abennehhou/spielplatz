using System.Threading.Tasks;
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
        public async Task GetByIdReturnsExpectedItem(Item expectedItem, ObjectId id)
        {
            // Arrange
            var itemsRepositoryMock = new Mock<IItemsRepository>();
            var itemsService = new ItemsService(itemsRepositoryMock.Object);
            itemsRepositoryMock.Setup(x => x.GetById(id)).ReturnsAsync(expectedItem);

            // Act
            var result = await itemsService.GetById(id.ToString());

            // Assert
            Assert.Equal(result, expectedItem);
            itemsRepositoryMock.VerifyAll();
        }
    }
}
