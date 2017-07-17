namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DetailsTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_query_for_details(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Course course)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            course.Id = 1234;
            course.Department = dept;

            await InsertAsync(dept, course);

            var result = await SendAsync(new Details.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }
    }
}