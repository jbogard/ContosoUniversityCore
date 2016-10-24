namespace ContosoUniversityCore.UnitTests
{
    using Shouldly;

    public class CalculatorTests
    {
        public void ShouldAdd() 
        {
            (1+1).ShouldBe(2);
        }
    }
}
