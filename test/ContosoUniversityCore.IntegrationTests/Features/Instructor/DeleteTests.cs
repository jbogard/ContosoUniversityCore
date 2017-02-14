namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;
    using static SliceFixture;

    public class DeleteTests
    {
        public async Task Should_query_for_command()
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
            var command = new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
                SelectedCourses = new []{ english101.Id.ToString()}
            };
            var instructorId = await SendAsync(command);

            await InsertAsync(englishDept, english101);

            var result = await SendAsync(new Delete.Query { Id = instructorId });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(command.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(command.OfficeAssignmentLocation);
        }

        public async Task Should_delete_instructor()
        {
            var instructorId = await SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
            });
            var englishDept = new Department
            {
                Name = "English",
                StartDate = DateTime.Today,
                InstructorID = instructorId
            };
            var english101 = new Course
            {
                Department = englishDept,
                Title = "English 101",
                Credits = 4,
                Id = 123
            };

            await InsertAsync(englishDept, english101);

            await SendAsync(new CreateEdit.Command
            {
                Id = instructorId,
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
                SelectedCourses = new[] { english101.Id.ToString() }
            });

            await SendAsync(new Delete.Command { ID = instructorId });

            var instructorCount = await ExecuteDbContextAsync(db => db.Instructors.CountAsync());

            instructorCount.ShouldBe(0);

            var englishDeptId = englishDept.Id;
            englishDept = await ExecuteDbContextAsync(db => db.Departments.FindAsync(englishDeptId));

            englishDept.InstructorID.ShouldBeNull();

            var courseInstructorCount = await ExecuteDbContextAsync(db => db.CourseInstructors.CountAsync());

            courseInstructorCount.ShouldBe(0);
        }

    }
}