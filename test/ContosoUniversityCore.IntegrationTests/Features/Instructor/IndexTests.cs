namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_get_list_instructor_with_details(Department dept, Course course, Course course2, CreateEdit.Command instructor, CreateEdit.Command instructor2, Student student1, Student student2)
        {
            course.Id = 123;
            course.Department = dept;
            course2.Id = 456;
            course2.Department = dept;                     

            await InsertAsync(dept, course, course2);

            instructor.SelectedCourses = new[] { course.Id.ToString(), course2.Id.ToString() };

            var instructor1Id = await SendAsync(instructor);

            await SendAsync(instructor2);       

            await InsertAsync(student1, student2);

            var enrollment1 = new Enrollment { StudentID = student1.Id, CourseID = course.Id };
            var enrollment2 = new Enrollment { StudentID = student2.Id, CourseID = course.Id };

            await InsertAsync(enrollment1, enrollment2);

            var result = await SendAsync(new Index.Query { Id = instructor1Id, CourseID = course.Id });

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