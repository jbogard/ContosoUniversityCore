namespace ContosoUniversityCore.Features.Department
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Infrastructure;
    using MediatR;

    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public string Name { get; set; }

            public decimal Budget { get; set; }

            public DateTime StartDate { get; set; }

            public int DepartmentID { get; set; }

            [Display(Name = "Administrator")]
            public string AdministratorFullName { get; set; }

        }

        public class QueryHandler : IAsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _context;

            public QueryHandler(SchoolContext context)
            {
                _context = context;
            }

            public async Task<Model> Handle(Query message)
            {
                string query = @"
SELECT d.*, p.LastName + ', ' + p.FirstName AS [AdministratorFullName]
FROM Department d
LEFT OUTER JOIN Person p ON d.InstructorID = p.ID
WHERE d.DepartmentID = @p0";
                Model department = await _context.Database.SqlQuery<Model>(query, message.Id).SingleOrDefaultAsync();

                return department;
            }
        }
    }
}