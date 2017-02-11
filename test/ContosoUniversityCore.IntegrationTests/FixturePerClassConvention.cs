namespace ContosoUniversityCore.IntegrationTests
{
    using Fixie;

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
        }
    }
}