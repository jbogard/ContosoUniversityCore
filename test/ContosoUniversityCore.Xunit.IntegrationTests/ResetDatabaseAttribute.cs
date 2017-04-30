namespace ContosoUniversityCore.Xunit.IntegrationTests
{
    using System.Reflection;
    using global::Xunit.Sdk;

    public class ResetDatabaseAttribute : BeforeAfterTestAttribute
    {
        public override void Before(MethodInfo methodUnderTest)
        {
            SliceFixture.ResetCheckpoint();
        }
    }
}