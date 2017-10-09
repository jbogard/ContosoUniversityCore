namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Fact]
        public async Task Should_return_all_courses()
        {
            var adminId = await SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await FindAsync<Instructor>(adminId);

            var englishDept = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var historyDept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var english = new Course
            {
                Credits = 4,
                Department = englishDept,
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = NextCourseNumber(),
                Title = "History 101"
            };
            await InsertAsync(englishDept, historyDept, english, history);

            var result = await SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Should_filter_courses()
        {
            var adminId = await SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await FindAsync<Instructor>(adminId);

            var englishDept = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var historyDept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            var english = new Course
            {
                Credits = 4,
                Department = englishDept,
                Id = NextCourseNumber(),
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = NextCourseNumber(),
                Title = "History 101"
            };
            await InsertAsync(englishDept, historyDept, english, history);

            var result = await SendAsync(new Index.Query {SelectedDepartment = englishDept});

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(1);
            result.Courses[0].Id.ShouldBe(english.Id);
        }
    }
}