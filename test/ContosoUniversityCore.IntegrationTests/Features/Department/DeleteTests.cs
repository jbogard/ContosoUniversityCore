namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_delete_department(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            await InsertAsync(dept);

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await SendAsync(command);

            var any = await ExecuteDbContextAsync(db => db.Departments.AnyAsync());

            any.ShouldBeFalse();
        }
    }
}