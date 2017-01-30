namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using global::Xunit;
    using Shouldly;

    public class IndexTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _fixture;

        public IndexTests(SliceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_return_all_courses()
        {
            var adminId = await _fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await _fixture.FindAsync<Instructor>(adminId);

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
                Id = 1235,
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = 4312,
                Title = "History 101"
            };
            await _fixture.InsertAsync(englishDept, historyDept, english, history);

            var result = await _fixture.SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);
        }
        [Fact]
        [ResetDatabase]
        public async Task Should_filter_courses()
        {
            var adminId = await _fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await _fixture.FindAsync<Instructor>(adminId);

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
                Id = 1235,
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                Id = 4312,
                Title = "History 101"
            };
            await _fixture.InsertAsync(englishDept, historyDept, english, history);

            var result = await _fixture.SendAsync(new Index.Query {SelectedDepartment = englishDept});

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(1);
            result.Courses[0].Id.ShouldBe(english.Id);
        }
    }
}