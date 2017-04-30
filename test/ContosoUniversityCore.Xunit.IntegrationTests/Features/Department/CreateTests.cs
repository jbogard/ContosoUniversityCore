namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using global::Xunit;
    using Shouldly;

    public class CreateTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _fixture;

        public CreateTests(SliceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_create_new_department()
        {
            var adminId = await _fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await _fixture.FindAsync<Instructor>(adminId);

            var command = new Create.Command
            {
                Budget = 10m,
                Name = "Engineering",
                StartDate = DateTime.Now.Date,
                Administrator = admin
            };

            await _fixture.SendAsync(command);

            var created = await _fixture.ExecuteDbContextAsync(db => db.Departments.Where(d => d.Name == command.Name).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            created.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            created.InstructorID.ShouldBe(admin.Id);
        }
    }
}