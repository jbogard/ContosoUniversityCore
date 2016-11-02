namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;

    public class DeleteTests
    {
        public async Task Should_delete_department(SliceFixture fixture)
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
            await fixture.InsertAsync(admin, dept);

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await fixture.SendAsync(command);

            await fixture.ExecuteDbContextAsync(async db =>
            {
                var any = await db.Departments.AnyAsync();

                any.ShouldBeFalse();
            });
        }
    }
}