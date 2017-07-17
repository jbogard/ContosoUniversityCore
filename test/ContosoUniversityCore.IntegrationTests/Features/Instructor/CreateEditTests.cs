namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class CreateEditTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_create_new_instructor(Department dept, Course course, Course course2, CreateEdit.Command command)
        {
            course.Id = 123;
            course.Department = dept;
            course2.Id = 456;
            course2.Department = dept;

            await InsertAsync(dept, course, course2);

            command.SelectedCourses = new[] { course.Id.ToString(), course2.Id.ToString() };

            await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Instructors.Include(i => i.CourseInstructors).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            created.FirstMidName.ShouldBe(command.FirstMidName);
            created.LastName.ShouldBe(command.LastName);
            created.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            created.OfficeAssignment.ShouldNotBeNull();
            created.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
            created.CourseInstructors.Count.ShouldBe(2);
        }

        [Theory, ConstruktionData]
        public async Task Should_edit_instructor_details(Department dept, Course course, Course course2, CreateEdit.Command instructor, CreateEdit.Command command)
        {
            course.Id = 123;
            course.Department = dept;
            course2.Id = 456;
            course2.Department = dept;

            await InsertAsync(dept, course, course2);

            var instructorId = await SendAsync(instructor);

            command.Id = instructorId;

            await SendAsync(command);

            var edited = await ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseInstructors).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            edited.FirstMidName.ShouldBe(command.FirstMidName);
            edited.LastName.ShouldBe(command.LastName);
            edited.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            edited.OfficeAssignment.ShouldNotBeNull();
            edited.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
        }

        [Theory, ConstruktionData]
        public async Task Should_merge_course_instructors(Department dept, Course course, Course course2, CreateEdit.Command instructor, CreateEdit.Command command)
        {
            course.Id = 123;
            course.Department = dept;
            course2.Id = 456;
            course2.Department = dept;

            await InsertAsync(dept, course, course2);

            instructor.SelectedCourses = new[] { course.Id.ToString() };
            var instructorId = await SendAsync(instructor);

            command.Id = instructorId;
            command.SelectedCourses = new[] { course2.Id.ToString() };

            await SendAsync(command);

            var edited = await ExecuteDbContextAsync(db => db.Instructors.Where(i => i.Id == instructorId).Include(i => i.CourseInstructors).Include(i => i.OfficeAssignment).SingleOrDefaultAsync());

            edited.FirstMidName.ShouldBe(command.FirstMidName);
            edited.LastName.ShouldBe(command.LastName);
            edited.HireDate.ShouldBe(command.HireDate.GetValueOrDefault());
            edited.OfficeAssignment.ShouldNotBeNull();
            edited.OfficeAssignment.Location.ShouldBe(command.OfficeAssignmentLocation);
            edited.CourseInstructors.Count.ShouldBe(1);
            edited.CourseInstructors.First().CourseID.ShouldBe(course2.Id);
        }

    }
}