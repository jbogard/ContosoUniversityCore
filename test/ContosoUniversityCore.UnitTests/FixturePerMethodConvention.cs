namespace ContosoUniversityCore.UnitTests
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

            Parameters.Add(
                mi =>
                    (mi.GetParameters().Length == 1) &&
                    (mi.GetParameters()[0].ParameterType == typeof(ContainerFixture))
                        ? new[] {new[] {new ContainerFixture()}}
                        : null);
        }
    }
}