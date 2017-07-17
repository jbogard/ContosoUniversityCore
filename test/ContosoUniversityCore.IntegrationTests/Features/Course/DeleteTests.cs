namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_query_for_command(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Course course)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            course.Id = 1234;
            course.Department = dept;

            await InsertAsync(dept, course);

            var result = await SendAsync(new Delete.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }

        [Theory, ConstruktionData]
        public async Task Should_delete(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Course course)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            course.Id = 1234;
            course.Department = dept;

            await InsertAsync(dept, course);

            await SendAsync(new Delete.Command {Id = course.Id});

            var result = await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == course.Id).SingleOrDefaultAsync());

            result.ShouldBeNull();
        }
    }
}