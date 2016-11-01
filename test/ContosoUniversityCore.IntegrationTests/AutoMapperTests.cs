namespace ContosoUniversityCore.IntegrationTests
{
    using AutoMapper;

    public class AutoMapperTests
    {
        public void Should_have_valid_configuration(SliceFixture fixture) 
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
