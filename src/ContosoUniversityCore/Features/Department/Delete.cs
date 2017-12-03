namespace ContosoUniversityCore.Features.Department
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Infrastructure;
    using MediatR;
    using System.Linq;
    using AutoMapper;

    public class Delete
    {
        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int Id { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

            public byte[] RowVersion { get; set; }
        }

        public class QueryHandler : AsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db)
            {
                _db = db;
            }

            protected override async Task<Command> HandleCore(Query message)
            {
                var department = await _db.Departments
                    .Where(d => d.Id == message.Id)
                    .ProjectToSingleOrDefaultAsync<Command>();

                return department;
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db)
            {
                _db = db;
            }

            protected override async Task HandleCore(Command message)
            {
                var department = await _db.Departments.FindAsync(message.Id);

                _db.Departments.Remove(department);
            }
        }
    }
}