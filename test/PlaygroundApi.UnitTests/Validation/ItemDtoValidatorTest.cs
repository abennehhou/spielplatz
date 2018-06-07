using System;
using System.Linq;
using Bogus;
using NUnit.Framework;
using Playground.Dto;
using PlaygroundApi.Validation;

namespace PlaygroundApi.UnitTests.Validation
{
    [TestFixture]
    public class ItemDtoValidatorTest
    {
        [TestCase("")]
        [TestCase(null)]
        public void ValidateItemDto_ReturnsError_WhenNameIsNotProvided(string name)
        {
            // Arrange
            var validator = new ItemDtoValidator();
            var testItems = new Faker<ItemDto>()
                .RuleFor(x => x.Name, f => name)
                .RuleFor(x => x.Id, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Owner, f => f.Name.FindName())
                .RuleFor(x => x.Tags, f => f.Lorem.Words().ToList());

            var item = testItems.Generate();

            // Act
            var result = validator.Validate(item);

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(1));
            Assert.That(result.Errors.Any(x => x.ErrorMessage.Contains(nameof(item.Name))), Is.True);
        }

        [Test]
        public void ValidateItemDto_ReturnsNoError_WithValidParameters()
        {
            // Arrange
            var validator = new ItemDtoValidator();
            var testItems = new Faker<ItemDto>()
                .RuleFor(x => x.Name, f => f.Commerce.Product())
                .RuleFor(x => x.Id, f => Guid.NewGuid().ToString())
                .RuleFor(x => x.Description, f => f.Lorem.Paragraph())
                .RuleFor(x => x.Owner, f => f.Name.FindName())
                .RuleFor(x => x.Tags, f => f.Lorem.Words().ToList());

            var item = testItems.Generate();

            // Act
            var result = validator.Validate(item);

            // Assert
            Assert.That(result.Errors.Count, Is.EqualTo(0));
        }
    }
}
