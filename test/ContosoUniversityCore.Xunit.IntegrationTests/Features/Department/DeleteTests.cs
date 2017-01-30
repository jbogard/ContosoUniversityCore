namespace ContosoUniversityCore.Xunit.IntegrationTests.Features.Department
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using global::Xunit;
    using Shouldly;

    public class DeleteTests : IClassFixture<SliceFixture>
    {
        private readonly SliceFixture _fixture;

        public DeleteTests(SliceFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task Should_delete_department()
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
            await _fixture.InsertAsync(dept);

            var command = new Delete.Command
            {
                Id = dept.Id,
                RowVersion = dept.RowVersion
            };

            await _fixture.SendAsync(command);

            var any = await _fixture.ExecuteDbContextAsync(db => db.Departments.AnyAsync());

            any.ShouldBeFalse();
        }
    }
}