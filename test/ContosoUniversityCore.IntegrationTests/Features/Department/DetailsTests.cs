namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;
    using static SliceFixture;

    public class DetailsTests
    {
        public async Task Should_get_department_details()
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