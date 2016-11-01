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

            var command = new Delete.Command
            {
                DepartmentID = dept.DepartmentID,
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