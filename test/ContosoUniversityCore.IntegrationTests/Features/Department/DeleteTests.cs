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
            var adminId = await fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await fixture.FindAsync<Instructor>(adminId);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            await fixture.InsertAsync(dept);

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await fixture.SendAsync(command);

            var any = await fixture.ExecuteDbContextAsync(db => db.Departments.AnyAsync());

            any.ShouldBeFalse();
        }
    }
}