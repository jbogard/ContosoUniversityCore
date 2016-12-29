namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;

    public class CreateTests
    {
        public async Task Should_create_new_department(SliceFixture fixture)
        {
            var adminId = await fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await fixture.FindAsync<Instructor>(adminId);

            var command = new Create.Command
            {
                Budget = 10m,
                Name = "Engineering",
                StartDate = DateTime.Now.Date,
                Administrator = admin
            };

            await fixture.SendAsync(command);

            var created = await fixture.ExecuteDbContextAsync(db => db.Departments.Where(d => d.Name == command.Name).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            created.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            created.InstructorID.ShouldBe(admin.Id);
        }
    }
}