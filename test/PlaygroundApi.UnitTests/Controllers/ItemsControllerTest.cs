using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using Playground.Domain;
using Playground.Dto;
using Playground.Services;
using PlaygroundApi.Controllers;

namespace PlaygroundApi.UnitTests.Controllers
{
    [TestFixture]
    public class ItemsControllerTest
    {
        private Mock<IItemsService> _itemsServiceMock;
        private Mock<ILogger<ItemsController>> _loggerMock;
        private Mock<IMapper> _mapperMock;
        private ItemsController _itemsController;
        private Faker _faker;

        [SetUp]
        public void Initialize()
        {
            _faker = new Faker();
            _itemsServiceMock = new Mock<IItemsService>();
            _loggerMock = new Mock<ILogger<ItemsController>>();
            _mapperMock = new Mock<IMapper>();
            _itemsController = new ItemsController(_itemsServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetById_ReturnsExpectedItem()
        {
            // Arrange
            var id = ObjectId.GenerateNewId().ToString();
            var item = new Item();

            var itemDto = new Faker<ItemDto>()
                .RuleFor(x => x.Name, f => f.Commerce.Product())
                .RuleFor(x => x.Id, f => id)
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Owner, f => f.Name.FindName())
                .RuleFor(x => x.Tags, f => f.Lorem.Words().ToList())
                .Generate();
            _itemsServiceMock.Setup(x => x.GetById(id)).ReturnsAsync(item);
            _mapperMock.Setup(x => x.Map<ItemDto>(item)).Returns(itemDto);

            // Act
            var result = await _itemsController.GetById(id) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            var value = result.Value;
            Assert.That(value, Is.EqualTo(itemDto));
        }
    }
}
