namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class CreateTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_create_new_department(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Create.Command command)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            command.Administrator = admin;

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Departments.Where(d => d.Name == command.Name).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            created.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            created.InstructorID.ShouldBe(admin.Id);
        }
    }
}