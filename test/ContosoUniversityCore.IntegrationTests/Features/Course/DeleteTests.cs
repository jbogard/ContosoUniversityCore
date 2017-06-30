namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
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
                Id = 1234,
                Title = "English 101"
            };

            await InsertAsync(dept, course);

            var result = await SendAsync(new Delete.Query {Id = course.Id});

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }

        [Fact]
        public async Task Should_delete()
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
                Id = 1234,
                Title = "English 101"
            };

            await InsertAsync(dept, course);

            await SendAsync(new Delete.Command {Id = course.Id});

            var result = await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id == course.Id).SingleOrDefaultAsync());

            result.ShouldBeNull();
        }
    }
}