namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using static SliceFixture;

    public class IndexTests
    {
        public async Task Should_list_departments()
        {
            var adminId = await SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await FindAsync<Instructor>(adminId);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var dept2 = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 456m,
                StartDate = DateTime.Today
            };

            await InsertAsync(dept, dept2);

            var query = new Index.Query();

            var result = await SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
        }

    }
}