namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Course
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
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
        public async Task Should_create_new_course()
        {
            var adminId = await _fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await _fixture.FindAsync<Instructor>(adminId);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await _fixture.InsertAsync(dept);

            var command = new Create.Command
            {
                Credits = 4,
                Department = dept,
                Number = 1234,
                Title = "English 101"
            };
            await _fixture.SendAsync(command);

            var created = await _fixture.ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == command.Number).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DepartmentID.ShouldBe(dept.Id);
            created.Credits.ShouldBe(command.Credits);
            created.Title.ShouldBe(command.Title);
        }
    }
}