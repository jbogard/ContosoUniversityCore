namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class EditTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_query_for_command()
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

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            await InsertAsync(dept, course);

            var result = await SendAsync(new Edit.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.Id.ShouldBe(dept.Id);
            result.Title.ShouldBe(course.Title);
        }

        [Fact]
        public async Task Should_edit()
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
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            await InsertAsync(dept, newDept, course);

            var command = new Edit.Command
            {
                Id = course.Id,
                Credits = 5,
                Department = newDept,
                Title = "English 202"
            };
            await SendAsync(command);

            var edited = await FindAsync<Course>(course.Id);

            edited.ShouldNotBeNull();
            edited.DepartmentID.ShouldBe(newDept.Id);
            edited.Credits.ShouldBe(command.Credits.GetValueOrDefault());
            edited.Title.ShouldBe(command.Title);
        }
    }
}