namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Department
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
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
        public async Task Should_list_departments()
        {
            var adminId = await _fixture.SendAsync(new ContosoUniversityCore.Features.Instructor.CreateEdit.Command
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            });
            var admin = await _fixture.FindAsync<Instructor>(adminId);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            var dept2 = new Department
            {
                Name = "English",
                Administrator = admin,
                Budget = 456m,
                StartDate = DateTime.Today
            };

            await _fixture.InsertAsync(dept, dept2);

            var query = new Index.Query();

            var result = await _fixture.SendAsync(query);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(2);
        }

    }
}