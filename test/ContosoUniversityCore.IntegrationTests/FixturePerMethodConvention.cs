namespace ContosoUniversityCore.IntegrationTests
{
    using Fixie;

    public class FixturePerMethodConvention : Convention
    {
        public FixturePerMethodConvention()
        {
            Classes
                .IsBddStyleClassNameOrEndsWithTests()
                .ConstructorDoesntHaveArguments();

            ClassExecution
                .CreateInstancePerCase();

            FixtureExecution
                .Wrap<DeleteData>();
        }
    }
}