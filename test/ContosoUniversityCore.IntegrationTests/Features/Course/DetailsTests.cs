namespace ContosoUniversityCore.IntegrationTests.Features.Course
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Course;
    using Domain;
    using Shouldly;

    public class DetailsTests
    {
        public async Task Should_query_for_details(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };

            await fixture.InsertAsync(admin);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };

            await fixture.InsertAsync(dept);

            var course = new Course
            {
                Credits = 4,
                Department = dept,
                Id = 1234,
                Title = "English 101"
            };

            await fixture.InsertAsync(course);

            var result = await fixture.SendAsync(new Details.Query { Id = course.Id });

            result.ShouldNotBeNull();
            result.Credits.ShouldBe(course.Credits);
            result.DepartmentName.ShouldBe(dept.Name);
            result.Title.ShouldBe(course.Title);
        }
    }
}