using AutoFixture;
using AutoFixture.Xunit2;

namespace Services.Tests
{
    public class CustomAutoDataAttribute : AutoDataAttribute
    {
        public CustomAutoDataAttribute()
            : base(() => new Fixture().Customize(new AutoFixtureConventions()))
        {
        }
    }
}
