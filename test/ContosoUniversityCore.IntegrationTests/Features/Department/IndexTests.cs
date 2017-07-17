namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_list_departments(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Department dept2)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;
            dept.Administrator = admin;

            await InsertAsync(dept, dept2);

            var query = new Index.Query();

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
        }

    }
}