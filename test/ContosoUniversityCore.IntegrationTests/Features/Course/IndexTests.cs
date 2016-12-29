namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;

    public class IndexTests
    {
        public async Task Should_return_all_courses(SliceFixture fixture)
        {
            var adminId = await fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await fixture.FindAsync<Instructor>(adminId);

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
            await fixture.InsertAsync(englishDept, historyDept, english, history);

            var result = await fixture.SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);
        }

        public async Task Should_filter_courses(SliceFixture fixture)
        {
            var adminId = await fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await fixture.FindAsync<Instructor>(adminId);

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
            await fixture.InsertAsync(englishDept, historyDept, english, history);

            var result = await fixture.SendAsync(new Index.Query {SelectedDepartment = englishDept});

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(1);
            result.Courses[0].Id.ShouldBe(english.Id);
        }
    }
}