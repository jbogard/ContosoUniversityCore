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
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Instructors.Add(admin);
                await db.SaveChangesAsync();
            });

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

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(englishDept);
                db.Departments.Add(historyDept);
                await db.SaveChangesAsync();
            });

            var english = new Course
            {
                Credits = 4,
                Department = englishDept,
                CourseID = 1235,
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                CourseID = 4312,
                Title = "History 101"
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Courses.Add(english);
                db.Courses.Add(history);
                await db.SaveChangesAsync();
            });

            var result = await fixture.SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);
        }

        public async Task Should_filter_courses(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Instructors.Add(admin);
                await db.SaveChangesAsync();
            });

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

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(englishDept);
                db.Departments.Add(historyDept);
                await db.SaveChangesAsync();
            });

            var english = new Course
            {
                Credits = 4,
                Department = englishDept,
                CourseID = 1235,
                Title = "English 101"
            };
            var history = new Course
            {
                Credits = 4,
                Department = historyDept,
                CourseID = 4312,
                Title = "History 101"
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Courses.Add(english);
                db.Courses.Add(history);
                await db.SaveChangesAsync();
            });

            var result = await fixture.SendAsync(new Index.Query {SelectedDepartment = englishDept});

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(1);
            result.Courses[0].CourseID.ShouldBe(english.CourseID);
        }
    }
}