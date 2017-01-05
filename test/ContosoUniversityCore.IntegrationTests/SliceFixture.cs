namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Domain;
    using FakeItEasy;
    using Infrastructure;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyModel;
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

            FixEntryAssembly();

            var startup = new Startup(host);
            _configuration = startup.Configuration;
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            var provider = services.BuildServiceProvider();
            _scopeFactory = provider.GetService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint();
        }

        private static void FixEntryAssembly()
        {
            // http://dejanstojanovic.net/aspnet/2015/january/set-entry-assembly-in-unit-testing-methods/
            AppDomainManager manager = new AppDomainManager();
            FieldInfo entryAssemblyfield = manager.GetType()
                .GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
            entryAssemblyfield?.SetValue(manager, typeof(Startup).Assembly);

            AppDomain domain = AppDomain.CurrentDomain;
            FieldInfo domainManagerField = domain.GetType()
                .GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
            domainManagerField?.SetValue(domain, manager);
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

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<SchoolContext>();

                try
                {
                    dbContext.BeginTransaction();

                    var result = await action(scope.ServiceProvider);

                    await dbContext.CommitTransactionAsync();

                    return result;
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

        public Task<T> ExecuteDbContextAsync<T>(Func<SchoolContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<SchoolContext>()));
        }

        public Task InsertAsync(params IEntity[] entities)
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (var entity in entities)
                {
                    db.Set(entity.GetType()).Add(entity);
                }
                return db.SaveChangesAsync();
            });
        }

        public Task<T> FindAsync<T>(int id)
            where T : class, IEntity
        {
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id));
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}