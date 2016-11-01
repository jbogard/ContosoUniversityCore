namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;

    public class DetailsTests
    {
        public async Task Should_get_department_details(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Instructors.Add(admin);
                await db.SaveChangesAsync();
            });

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(dept);
                await db.SaveChangesAsync();
            });

            var query = new Details.Query
            {
                Id = dept.DepartmentID
            };

            var result = await fixture.SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.AdministratorFullName.ShouldBe(admin.FullName);
        }

    }
}