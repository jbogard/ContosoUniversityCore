namespace ContosoUniversityCore.UnitTests
{
    using AutoMapper;
    using Shouldly;

    public class AutoMapperTestsff
    {
        public void Should_have_valid_configuration() 
        {
            Mapper.AssertConfigurationIsValid();
        }
    }
}
