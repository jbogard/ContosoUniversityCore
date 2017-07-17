namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class EditTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_query_for_command(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Course course)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;

            course.Id = 1234;
            course.Department = dept;

            await InsertAsync(dept, course);

            var result = await SendAsync(new Edit.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.Department.Id.ShouldBe(dept.Id);
            result.Title.ShouldBe(course.Title);
        }

        [Theory, ConstruktionData]
        public async Task Should_edit(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Department newDept, Course course, Edit.Command command)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;
            newDept.Administrator = admin;

            course.Id = 1234;
            course.Department = dept;    
       
            await InsertAsync(dept, newDept, course);

            command.Id = course.Id;
            command.Department = newDept;

            await SendAsync(command);

            var edited = await FindAsync<Course>(course.Id);

            edited.ShouldNotBeNull();
            edited.DepartmentID.ShouldBe(newDept.Id);
            edited.Credits.ShouldBe(command.Credits.GetValueOrDefault());
            edited.Title.ShouldBe(command.Title);
        }
    }
}