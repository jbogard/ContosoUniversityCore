namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;

    public class IndexTests
    {
        public async Task Should_get_list_instructor_with_details(SliceFixture fixture)
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
            var instructor1 = new Instructor
            {
                OfficeAssignment = new OfficeAssignment { Location = "Austin" },
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            var instructor2 = new Instructor
            {
                OfficeAssignment = new OfficeAssignment { Location = "Houston" },
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
            };
            instructor1.CourseInstructors.Add(new CourseInstructor { Course = english101, Instructor = instructor1 });
            instructor1.CourseInstructors.Add(new CourseInstructor { Course = english201, Instructor = instructor1 });

            await fixture.InsertAsync(englishDept, english101, english201, instructor1, instructor2);

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
            student1.Enrollments.Add(new Enrollment {Student = student1, Course = english101});
            student2.Enrollments.Add(new Enrollment {Student = student2, Course = english101});

            await fixture.InsertAsync(student1, student2);

            var result = await fixture.SendAsync(new Index.Query {Id = instructor1.Id, CourseID = english101.Id});

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