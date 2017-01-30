namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Instructor
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
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
        public async Task Should_get_list_instructor_with_details()
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
            var english201 = new Course
            {
                Department = englishDept,
                Title = "English 201",
                Credits = 4,
                Id = 456
            };

            await _fixture.InsertAsync(englishDept, english101, english201);

            var instructor1Id = await _fixture.SendAsync(new CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                SelectedCourses = new[] { english101.Id.ToString(), english201.Id.ToString() },
                HireDate = DateTime.Today,
                OfficeAssignmentLocation = "Austin",
            });

            await _fixture.SendAsync(new CreateEdit.Command
            {
                OfficeAssignmentLocation = "Houston",
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
            });

            var student1 = new Student
            {
                FirstMidName = "Cosmo",
                LastName = "Kramer",
                EnrollmentDate = DateTime.Today,
            };
            var student2 = new Student
            {
                FirstMidName = "Elaine",
                LastName = "Benes",
                EnrollmentDate = DateTime.Today
            };

            await _fixture.InsertAsync(student1, student2);

            var enrollment1 = new Enrollment { StudentID = student1.Id, CourseID = english101.Id };
            var enrollment2 = new Enrollment { StudentID = student2.Id, CourseID = english101.Id };

            await _fixture.InsertAsync(enrollment1, enrollment2);

            var result = await _fixture.SendAsync(new Index.Query { Id = instructor1Id, CourseID = english101.Id });

            result.ShouldNotBeNull();

            result.Instructors.ShouldNotBeNull();
            result.Instructors.Count.ShouldBe(2);

            result.Courses.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);

            result.Enrollments.ShouldNotBeNull();
            result.Enrollments.Count.ShouldBe(2);
        }

    }
}