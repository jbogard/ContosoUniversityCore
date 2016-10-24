namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using FakeItEasy;
    using Fixie;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;

    public class FixturePerClassConvention : Convention
    {
        public FixturePerClassConvention()
        {
            Classes
                .IsBddStyleClassNameOrEndsWithTests()
                .ConstructorHasArguments();

            ClassExecution
                .CreateInstancePerClass()
                .Wrap<DeleteData>();

            Parameters.Add(
                mi =>
                    (mi.GetParameters().Length == 1) &&
                    (mi.GetParameters()[0].ParameterType == typeof(ContainerFixture))
                        ? new[] {new[] {new ContainerFixture()}}
                        : null);
        }
    }

    public class FixturePerMethodConvention : Convention
    {
        public FixturePerMethodConvention()
        {
            Classes
                .IsBddStyleClassNameOrEndsWithTests()
                .ConstructorDoesntHaveArguments();

            ClassExecution
                .CreateInstancePerCase();

            Parameters.Add(
                mi =>
                    (mi.GetParameters().Length == 1) &&
                    (mi.GetParameters()[0].ParameterType == typeof(ContainerFixture))
                        ? new[] {new[] {new ContainerFixture()}}
                        : null);

            FixtureExecution
                .Wrap<DeleteData>();
        }
    }


    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            ContainerFixture.ResetCheckpoint();
            next();
        }

        public void Execute(Fixture context, Action next)
        {
            ContainerFixture.ResetCheckpoint();
            next();
        }
    }

    public class ContainerFixture
    {
        private static readonly Checkpoint _checkpoint;
        private static readonly IServiceProvider _rootContainer;
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static ContainerFixture()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            _configuration = startup.Configuration;
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            _rootContainer = services.BuildServiceProvider();
            _scopeFactory = _rootContainer.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint();
        }

        public static void ResetCheckpoint([CallerMemberName] string memberName = "")
        {
            _checkpoint.Reset(_configuration["Data:DefaultConnection:ConnectionString"]);
        }

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<SchoolContext>();

                try
                {
                    dbContext.BeginTransaction();

                    await action(scope.ServiceProvider);
                }
                catch (Exception ex)
                {
                    dbContext.CloseTransaction(ex);
                    throw;
                }
                dbContext.CloseTransaction();
            }
        }

        public Task ExecuteDbContextAsync(Func<SchoolContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));
        }

        public async Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request)
        {
            TResponse response = default(TResponse);
            await ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();

                response = await mediator.SendAsync(request);
            });
            return response;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            TResponse response = default(TResponse);
            await ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                response = mediator.Send(request);

                return Task.FromResult(0);
            });
            return response;
        }
    }
}