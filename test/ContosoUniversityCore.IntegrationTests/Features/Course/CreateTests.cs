namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;

    public class CreateTests
    {
        public async Task Should_create_new_course(SliceFixture fixture)
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

            var command = new Create.Command
            {
                Credits = 4,
                Department = dept,
                Number = 1234,
                Title = "English 101"
            };
            await fixture.SendAsync(command);

            var created = await fixture.ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == command.Number).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DepartmentID.ShouldBe(dept.Id);
            created.Credits.ShouldBe(command.Credits);
            created.Title.ShouldBe(command.Title);
        }
    }
}