namespace ContosoUniversityCore.Xunit.IntegrationTests
{
    using AutoMapper;
    using global::Xunit;

    public class AutomapperTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _fixture;

        public AutomapperTests(SliceFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void Should_have_valid_configuration()
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}