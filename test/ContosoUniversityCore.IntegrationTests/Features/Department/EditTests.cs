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

    public class EditTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_get_edit_department_details(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            await InsertAsync(dept);

            var query = new Edit.Query
            {
                Id = dept.Id
            };

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.Administrator.Id.ShouldBe(admin.Id);
        }

        [Theory, ConstruktionData]
        public async Task Should_edit_department(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor2, Department dept, Edit.Command command)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            var admin2Id = await SendAsync(instructor2);
            var admin2 = await FindAsync<Instructor>(admin2Id);

            dept.Administrator = admin;

            await InsertAsync(dept);

            command.Id = dept.Id;
            command.Administrator = admin;

            await SendAsync(command);

            var result = await ExecuteDbContextAsync(db => db.Departments.Where(d => d.Id == dept.Id).Include(d => d.Administrator).SingleOrDefaultAsync());

            result.Name.ShouldBe(command.Name);
            result.Administrator.Id.ShouldBe(command.Administrator.Id);
            result.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            result.Budget.ShouldBe(command.Budget.GetValueOrDefault());
        }
    }
}