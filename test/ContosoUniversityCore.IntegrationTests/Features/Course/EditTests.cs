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

            var result = await fixture.SendAsync(new Edit.Query { Id = course.CourseID });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.DepartmentID.ShouldBe(dept.DepartmentID);
            result.Title.ShouldBe(course.Title);
        }

        public async Task Should_edit(SliceFixture fixture)
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
            var newDept = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Departments.Add(dept);
                db.Departments.Add(newDept);
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

            var command = new Edit.Command
            {
                CourseID = course.CourseID,
                Credits = 5,
                Department = newDept,
                Title = "English 202"
            };
            await fixture.SendAsync(command);

            await fixture.ExecuteDbContextAsync(async db =>
            {
                var created = await db.Courses.Where(c => c.CourseID == command.CourseID).SingleOrDefaultAsync();

                created.ShouldNotBeNull();
                created.DepartmentID.ShouldBe(newDept.DepartmentID);
                created.Credits.ShouldBe(command.Credits.GetValueOrDefault());
                created.Title.ShouldBe(command.Title);
            });
        }
    }
}