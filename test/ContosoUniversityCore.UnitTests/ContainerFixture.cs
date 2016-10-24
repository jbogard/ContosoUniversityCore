namespace ContosoUniversityCore.UnitTests
{
    using System.IO;
    using FakeItEasy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public class ContainerFixture
    {
        static ContainerFixture()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
        }

    }
}