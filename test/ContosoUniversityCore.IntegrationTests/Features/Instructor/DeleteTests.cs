namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;

    public class DeleteTests
    {
        public async Task Should_query_for_command(SliceFixture fixture)
        {
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = 123
            };
            var instructor = new Instructor
            {
                OfficeAssignment = new OfficeAssignment { Location = "Austin" },
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            instructor.CourseInstructors.Add(new CourseInstructor { Course = english101, Instructor = instructor });

            await fixture.InsertAsync(englishDept, english101, instructor);

            var result = await fixture.SendAsync(new Delete.Query { Id = instructor.Id });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(instructor.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(instructor.OfficeAssignment.Location);
        }

        public async Task Should_delete_instructor(SliceFixture fixture)
        {
            var instructor = new Instructor
            {
                OfficeAssignment = new OfficeAssignment { Location = "Austin" },
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today,
                Administrator = instructor
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = 123
            };
            instructor.CourseInstructors.Add(new CourseInstructor { Course = english101, Instructor = instructor });

            await fixture.InsertAsync(instructor, englishDept, english101);

            await fixture.SendAsync(new Delete.Command { ID = instructor.Id });

            var instructorCount = await fixture.ExecuteDbContextAsync(db => db.Instructors.CountAsync());

            instructorCount.ShouldBe(0);

            var englishDeptId = englishDept.Id;
            englishDept = await fixture.ExecuteDbContextAsync(db => db.Departments.FindAsync(englishDeptId));

            englishDept.InstructorID.ShouldBeNull();

            var courseInstructorCount = await fixture.ExecuteDbContextAsync(db => db.CourseInstructors.CountAsync());

            courseInstructorCount.ShouldBe(0);
        }

    }
}