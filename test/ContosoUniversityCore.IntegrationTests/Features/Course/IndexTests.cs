namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;
    using Xunit;
    using static SliceFixture;

    public class IndexTests : IntegrationTestBase
    {
        [Theory, ConstruktionData]
        public async Task Should_return_all_courses(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Department dept2, Course course, Course course2)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;
            dept2.Administrator = admin;

            course.Id = 1235;
            course.Department = dept;
            course2.Id = 4312;
            course2.Department = dept2;

            await InsertAsync(dept, dept2, course, course2);

            var result = await SendAsync(new Index.Query());

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(2);
        }

        [Theory, ConstruktionData]
        public async Task Should_filter_courses(ContosoUniversityCore.Features.Instructor.CreateEdit.Command instructor, Department dept, Department dept2, Course course, Course course2)
        {
            var adminId = await SendAsync(instructor);
            var admin = await FindAsync<Instructor>(adminId);

            dept.Administrator = admin;
            dept2.Administrator = admin;

            course.Id = 1235;
            course.Department = dept;
            course2.Id = 4312;
            course2.Department = dept2;

            await InsertAsync(dept, dept2, course, course2);

            var result = await SendAsync(new Index.Query {SelectedDepartment = dept});

            result.ShouldNotBeNull();
            result.Courses.Count.ShouldBe(1);
            result.Courses[0].Id.ShouldBe(course.Id);
        }
    }
}