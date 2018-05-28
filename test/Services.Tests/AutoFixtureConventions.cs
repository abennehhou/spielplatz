using AutoFixture;
using AutoFixture.AutoMoq;
using MongoDB.Bson;

namespace Services.Tests
{
    internal class AutoFixtureConventions : CompositeCustomization
    {
        public AutoFixtureConventions()
            : base(new MongoObjectIdCustomization(), new AutoMoqCustomization())
        {
        }

        private class MongoObjectIdCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Register(ObjectId.GenerateNewId);
            }
        }
    }
}
