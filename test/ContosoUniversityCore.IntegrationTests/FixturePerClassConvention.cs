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

            Parameters.Add(
                mi =>
                    (mi.GetParameters().Length == 1) &&
                    (mi.GetParameters()[0].ParameterType == typeof(SliceFixture))
                        ? new[] {new[] {new SliceFixture()}}
                        : null);
        }
    }
}