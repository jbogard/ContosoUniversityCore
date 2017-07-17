namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DetailsTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_get_department_details(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            await InsertAsync(dept);

            var query = new Details.Query
            {
                Id = dept.Id
            };

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.AdministratorFullName.ShouldBe(admin.FullName);
        }

    }
}