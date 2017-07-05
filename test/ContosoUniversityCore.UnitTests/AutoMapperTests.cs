namespace ContosoUniversityCore.UnitTests
{
    using AutoMapper;
    using System.IO;
    using FakeItEasy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class AutoMapperTests
    {
        [Fact]
        public void Should_have_valid_configuration() 
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            //FixEntryAssembly();

            var startup = new Startup(host);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
