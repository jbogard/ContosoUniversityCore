namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;

    public class DetailsTests
    {
        public async Task Should_get_instructor_details(SliceFixture fixture)
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

            var result = await fixture.SendAsync(new Details.Query { Id = instructor.Id });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(instructor.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(instructor.OfficeAssignment.Location);
        }
    }
}