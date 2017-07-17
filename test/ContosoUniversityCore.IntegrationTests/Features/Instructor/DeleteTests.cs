namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class DeleteTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_query_for_command(Department dept, Course course, CreateEdit.Command command)
        {
            course.Id = 123;
            course.Department = dept;

            command.SelectedCourses = new[] { course.Id.ToString() };

            var instructorId = await SendAsync(command);

            await InsertAsync(dept, course);

            var result = await SendAsync(new Delete.Query { Id = instructorId });

            result.ShouldNotBeNull();
            result.FirstMidName.ShouldBe(command.FirstMidName);
            result.OfficeAssignmentLocation.ShouldBe(command.OfficeAssignmentLocation);
        }

        [Theory, ConstruktionData]
        public async Task Should_delete_instructor(CreateEdit.Command instructor, Department dept, Course course, CreateEdit.Command command)
        {           
            var instructorId = await SendAsync(instructor);
            dept.InstructorID = instructorId;
            course.Id = 123;
            course.Department = dept;

            await InsertAsync(dept, course);           

            command.Id = instructorId;
            command.SelectedCourses = new[] { course.Id.ToString() };

            await SendAsync(command);

            await SendAsync(new Delete.Command { ID = instructorId });

            var instructorCount = await ExecuteDbContextAsync(db => db.Instructors.CountAsync());

            instructorCount.ShouldBe(0);

            var deptId = dept.Id;
            dept = await ExecuteDbContextAsync(db => db.Departments.FindAsync(deptId));

            dept.InstructorID.ShouldBeNull();

            var courseInstructorCount = await ExecuteDbContextAsync(db => db.CourseInstructors.CountAsync());

            courseInstructorCount.ShouldBe(0);
        }

    }
}