using System;
using Bogus;
using NUnit.Framework;
using Playground.Dto;
using PlaygroundApi.Navigation;

namespace PlaygroundApi.UnitTests.Navigation
{
    [TestFixture]
    public class PagedListExtensionsTest
    {
        private Faker _faker;

        [SetUp]
        public void Initialize()
        {
            _faker = new Faker();
        }

        [Test]
        public void BuildNavigationLinks_BuildsSelfLink_WithValidUri()
        {
            // Arrange
            var url = _faker.Internet.UrlWithPath();
            var pagedList = new PagedListDto<string>();

            // Act
            pagedList.BuildNavigationLinks(url);

            // Assert
            Assert.That(pagedList.Links, Is.Not.Null);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameSelf), Is.True);
            Assert.That(pagedList.Links[ResourceBase.RelationNameSelf], Is.EqualTo(url));
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameNext), Is.False);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNamePrevious), Is.False);
        }

        [Test]
        public void BuildNavigationLinks_BuildsNextLink_WhenSkipIsNotProvided()
        {
            // Arrange
            var url = $"{_faker.Internet.UrlWithPath()}?{_faker.Lorem.Word()}={_faker.Lorem.Word()}";
            var expectedSkip = "skip=10";
            var expectedUrl = $"{url}&{expectedSkip}";
            var uri = new Uri(url);
            var pagedList = new PagedListDto<string>
            {
                HasNextPage = true,
                LastItemOnPage = 10
            };

            // Act
            pagedList.BuildNavigationLinks(uri);

            // Assert
            Assert.That(pagedList.Links, Is.Not.Null);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameSelf));
            Assert.That(pagedList.Links[ResourceBase.RelationNameSelf], Is.EqualTo(url));
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameNext), Is.True);
            var nextLink = pagedList.Links[ResourceBase.RelationNameNext];
            Assert.That(nextLink, Is.Not.Null);
            Assert.That(nextLink.ToLower(), Is.EqualTo(expectedUrl));
        }

        [Test]
        public void BuildNavigationLinks_BuildsNextLink_WhenSkipIsProvided()
        {
            // Arrange
            var urlPrefix = $"{_faker.Internet.UrlWithPath()}?{_faker.Lorem.Word()}={_faker.Lorem.Word()}";
            var url = $"{urlPrefix}&skip=10&limit=5";
            var expectedUrl = $"{urlPrefix}&skip=15&limit=5";
            var uri = new Uri(url);
            var pagedList = new PagedListDto<string>
            {
                HasNextPage = true,
                LastItemOnPage = 15
            };

            // Act
            pagedList.BuildNavigationLinks(uri);

            // Assert
            Assert.That(pagedList.Links, Is.Not.Null);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameSelf));
            Assert.That(pagedList.Links[ResourceBase.RelationNameSelf], Is.EqualTo(url));
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameNext), Is.True);
            var nextLink = pagedList.Links[ResourceBase.RelationNameNext];
            Assert.That(nextLink, Is.Not.Null);
            Assert.That(nextLink.ToLower(), Is.EqualTo(expectedUrl));
        }

        [Test]
        public void BuildNavigationLinks_BuildsPreviousLink_WhenPageHasPreviousPage()
        {
            // Arrange
            var urlPrefix = $"{_faker.Internet.UrlWithPath()}?{_faker.Lorem.Word()}={_faker.Lorem.Word()}";
            var url = $"{urlPrefix}&skip=10&limit=5";
            var expectedUrl = $"{urlPrefix}&skip=5&limit=5";
            var uri = new Uri(url);
            var pagedList = new PagedListDto<string>
            {
                HasPreviousPage = true,
                FirstItemOnPage = 11,
                PageSize = 5
            };

            // Act
            pagedList.BuildNavigationLinks(uri);

            // Assert
            Assert.That(pagedList.Links, Is.Not.Null);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameSelf));
            Assert.That(pagedList.Links[ResourceBase.RelationNameSelf], Is.EqualTo(url));
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNamePrevious), Is.True);
            var previousLink = pagedList.Links[ResourceBase.RelationNamePrevious];
            Assert.That(previousLink, Is.Not.Null);
            Assert.That(previousLink.ToLower(), Is.EqualTo(expectedUrl));
        }

        [Test]
        public void BuildNavigationLinks_BuildsAllLinks_WhenPagedListHasPreviousAndNextPage()
        {
            // Arrange
            var urlPrefix = $"{_faker.Internet.UrlWithPath()}?{_faker.Lorem.Word()}={_faker.Lorem.Word()}";
            var url = $"{urlPrefix}&skip=20&limit=5";
            var expectedPreviousUrl = $"{urlPrefix}&skip=15&limit=5";
            var expectedNextUrl = $"{urlPrefix}&skip=25&limit=5";
            var uri = new Uri(url);
            var pagedList = new PagedListDto<string>
            {
                HasPreviousPage = true,
                HasNextPage = true,
                FirstItemOnPage = 21,
                LastItemOnPage = 25,
                PageSize = 5
            };

            // Act
            pagedList.BuildNavigationLinks(uri);

            // Assert
            Assert.That(pagedList.Links, Is.Not.Null);
            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameSelf));
            Assert.That(pagedList.Links[ResourceBase.RelationNameSelf], Is.EqualTo(url));

            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNamePrevious), Is.True);
            var previousLink = pagedList.Links[ResourceBase.RelationNamePrevious];
            Assert.That(previousLink, Is.Not.Null);
            Assert.That(previousLink.ToLower(), Is.EqualTo(expectedPreviousUrl));

            Assert.That(pagedList.Links.ContainsKey(ResourceBase.RelationNameNext), Is.True);
            var nextLink = pagedList.Links[ResourceBase.RelationNameNext];
            Assert.That(nextLink, Is.Not.Null);
            Assert.That(nextLink.ToLower(), Is.EqualTo(expectedNextUrl));
        }
    }
}
