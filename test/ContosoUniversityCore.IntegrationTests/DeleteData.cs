namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using Fixie;

    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            ContainerFixture.ResetCheckpoint();
            next();
        }

        public void Execute(Fixture context, Action next)
        {
            ContainerFixture.ResetCheckpoint();
            next();
        }
    }
}