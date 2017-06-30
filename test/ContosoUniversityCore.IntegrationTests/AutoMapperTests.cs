using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ContosoUniversityCore.IntegrationTests
{
    using AutoMapper;

    public class AutoMapperTests
    {
        [Fact]
        public void Should_have_valid_configuration() 
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
