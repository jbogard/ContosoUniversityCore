namespace ContosoUniversityCore.IntegrationTests.Features.Department
{
    using System;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Department;
    using Domain;
    using Shouldly;

    public class EditTests
    {
        public async Task Should_get_edit_department_details(SliceFixture fixture)
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

            var query = new Edit.Query
            {
                Id = dept.Id
            };

            var result = await fixture.SendAsync(query);

            result.ShouldNotBeNull();
            result.Name.ShouldBe(dept.Name);
            result.Administrator.Id.ShouldBe(admin.Id);
        }

        public async Task Should_edit_department(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };
            var admin2 = new Instructor
            {
                FirstMidName = "Jerry",
                LastName = "Seinfeld",
                HireDate = DateTime.Today,
            };
            await fixture.InsertAsync(admin, admin2);

            var dept = new Department
            {
                Name = "History",
                Administrator = admin,
                Budget = 123m,
                StartDate = DateTime.Today
            };
            await fixture.InsertAsync(dept);

            var command = new Edit.Command
            {
                Id = dept.Id,
                Name = "English",
                Administrator = admin2,
                StartDate = DateTime.Today.AddDays(-1),
                Budget = 456m
            };

            await fixture.SendAsync(command);

            await fixture.ExecuteDbContextAsync(async db =>
            {
                var result = await db.Departments.FindAsync(dept.Id);

                result.Name.ShouldBe(command.Name);
                result.Administrator.Id.ShouldBe(command.Administrator.Id);
                result.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
                result.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            });
        }
    }
}