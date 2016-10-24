namespace ContosoUniversityCore.UnitTests
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
                .CreateInstancePerClass();

            Parameters.Add(
                mi =>
                    (mi.GetParameters().Length == 1) &&
                    (mi.GetParameters()[0].ParameterType == typeof(ContainerFixture))
                        ? new[] {new[] {new ContainerFixture()}}
                        : null);
        }
    }
}