namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using System.Linq;
    using System.Data.Entity;

    public class EditTests
    {
        public async Task Should_query_for_command(SliceFixture fixture)
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

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = 1234,
                Title = "English 101"
            };
            await fixture.InsertAsync(dept, course);

            var result = await fixture.SendAsync(new Edit.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.Id.ShouldBe(dept.Id);
            result.Title.ShouldBe(course.Title);
        }

        public async Task Should_edit(SliceFixture fixture)
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
            var newDept = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = 1234,
                Title = "English 101"
            };
            await fixture.InsertAsync(dept, newDept, course);

            var command = new Edit.Command
            {
                Id = course.Id,
                Credits = 5,
                Department = newDept,
                Title = "English 202"
            };
            await fixture.SendAsync(command);

            var edited = await fixture.FindAsync<Course>(course.Id);

            edited.ShouldNotBeNull();
            edited.DepartmentID.ShouldBe(newDept.Id);
            edited.Credits.ShouldBe(command.Credits.GetValueOrDefault());
            edited.Title.ShouldBe(command.Title);
        }
    }
}