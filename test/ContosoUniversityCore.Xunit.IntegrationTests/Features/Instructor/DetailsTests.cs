namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Instructor
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using global::Xunit;
    using Shouldly;

    public class DetailsTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _fixture;

        public DetailsTests(SliceFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task Should_get_instructor_details()
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
            await _fixture.InsertAsync(englishDept, english101);

            var command = new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                OfficeAssignmentLocation = "Austin",
                HireDate = DateTime.Today,
                SelectedCourses = new[] { english101.Id.ToString() }
            };
            var instructorId = await _fixture.SendAsync(command);

            var result = await _fixture.SendAsync(new Details.Query { Id = instructorId });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(command.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(command.OfficeAssignmentLocation);
        }
    }
}