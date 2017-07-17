namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DetailsTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_get_instructor_details(Department dept, Course course, CreateEdit.Command command)
        {
            course.Id = 123;
            course.Department = dept;

            await InsertAsync(dept, course);

            command.SelectedCourses = new[] { course.Id.ToString() };

            var instructorId = await SendAsync(command);

            var result = await SendAsync(new Details.Query { Id = instructorId });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(command.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(command.OfficeAssignmentLocation);
        }
    }
}