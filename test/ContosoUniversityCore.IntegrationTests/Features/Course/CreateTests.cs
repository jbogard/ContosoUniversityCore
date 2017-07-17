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

    public class CreateTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_create_new_course(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Create.Command command)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            await InsertAsync(dept);

            command.Department = dept;

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == command.Number).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DepartmentID.ShouldBe(dept.Id);
            created.Credits.ShouldBe(command.Credits);
            created.Title.ShouldBe(command.Title);
        }
    }
}