namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;

    public class IndexTests
    {
        public async Task Should_list_departments(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };

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

            await fixture.InsertAsync(admin, dept, dept2);

            var query = new Index.Query();

            var result = await fixture.SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
        }

    }
}