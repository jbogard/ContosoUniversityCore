namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using FakeItEasy;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Respawn;

    public class SliceFixture
    {
        private static readonly Checkpoint _checkpoint;
        private static readonly IConfigurationRoot _configuration;
        private static readonly IServiceScopeFactory _scopeFactory;

        static SliceFixture()
        {
            var host = A.Fake<IHostingEnvironment>();

            A.CallTo(() => host.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var startup = new Startup(host);
            _configuration = startup.Configuration;
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint();
        }

        public static void ResetCheckpoint()
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

                    await dbContext.CommitTransactionAsync();
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public Task ExecuteDbContextAsync(Func<SchoolContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));
        }

        public async Task<TResponse> SendAsync<TResponse>(IAsyncRequest<TResponse> request)
        {
            var response = default(TResponse);
            await ExecuteScopeAsync(async sp =>
            {
                var mediator = sp.GetService<IMediator>();

                response = await mediator.SendAsync(request);
            });
            return response;
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            var response = default(TResponse);
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