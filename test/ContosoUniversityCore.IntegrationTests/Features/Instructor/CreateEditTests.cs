namespace ContosoUniversityCore.IntegrationTests.Features.Instructor
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using ContosoUniversityCore.Features.Instructor;
    using Domain;
    using Shouldly;

    public class CreateEditTests
    {
        public async Task Should_create_new_instructor(SliceFixture fixture)
        {
            var admin = new Instructor
            {
                FirstMidName = "George",
                LastName = "Costanza",
                HireDate = DateTime.Today,
            };

            await fixture.ExecuteDbContextAsync(async db =>
            {
                db.Instructors.Add(admin);
                await db.SaveChangesAsync();
            });


            var command = new CreateEdit.Command
            {
                
            };

            //await fixture.SendAsync(command);

            //await fixture.ExecuteDbContextAsync(async db =>
            //{
            //    var created = await db.Departments.Where(d => d.Name == command.Name).SingleOrDefaultAsync();

            //    created.ShouldNotBeNull();
            //    created.Budget.ShouldBe(command.Budget.GetValueOrDefault());
            //    created.StartDate.ShouldBe(command.StartDate.GetValueOrDefault());
            //    created.InstructorID.ShouldBe(admin.ID);
            //});
        }
    }
}