using System;
using Moq;
using NUnit.Framework;
using PlaygroundApi.Mapping;

namespace PlaygroundApi.UnitTests.Mapping
{
    [TestFixture]
    public class AutoMapperConfigTest
    {
        [Test]
        public void ConfigureAutoMapper()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();

            // Act
            var mapper = AutoMapperConfig.Configure(serviceProviderMock.Object);

            // Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
