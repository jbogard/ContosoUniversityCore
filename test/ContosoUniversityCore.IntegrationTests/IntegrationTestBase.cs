using System.Threading.Tasks;
using Xunit;

namespace ContosoUniversityCore.IntegrationTests
{
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        public virtual Task InitializeAsync() => SliceFixture.ResetCheckpoint();

        public virtual Task DisposeAsync() => Task.FromResult(0);
    }
}