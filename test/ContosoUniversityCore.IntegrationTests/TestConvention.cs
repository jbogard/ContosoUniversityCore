using Fixie.Conventions;

namespace ContosoUniversityCore.IntegrationTests
{
    public class TestConvention : DefaultConvention
    {
        public TestConvention()
        {
            FixtureExecution
                .Wrap<DeleteData>();
        }
    }
}