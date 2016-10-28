namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;

    public class DeleteTests
    {
        public async Task Should_query_for_command(SliceFixture fixture)
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

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(dept);
                await db.SaveChangesAsync();
            });

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                CourseID = 1234,
                Title = "English 101"
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
            });

            var result = await fixture.SendAsync(new Delete.Query {Id = course.CourseID});

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }

        public async Task Should_delete(SliceFixture fixture)
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

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(dept);
                await db.SaveChangesAsync();
            });

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                CourseID = 1234,
                Title = "English 101"
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();
            });

            await fixture.SendAsync(new Delete.Command {CourseID = course.CourseID});

            await fixture.ExecuteDbContextAsync(async db =>
            {
                var result = await db.Courses.Where(c => c.CourseID == course.CourseID).SingleOrDefaultAsync();

                result.ShouldBeNull();
            });
        }
    }
}