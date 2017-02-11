namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using Fixie;
    using static SliceFixture;

    public class DeleteData : FixtureBehavior, ClassBehavior
    {
        public void Execute(Class context, Action next)
        {
            ResetCheckpoint();
            next();
        }

        public void Execute(Fixture context, Action next)
        {
            ResetCheckpoint();
            next();
        }
    }
}