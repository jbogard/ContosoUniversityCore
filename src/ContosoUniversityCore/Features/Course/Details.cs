namespace ContosoUniversityCore.Features.Course
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using FluentValidation;
    using Infrastructure;
    using MediatR;

    public class Details
    {
        public class Query : IAsyncRequest<Model>
        {
            public int? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Model
        {
            public int CourseID { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            public string DepartmentName { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<Model> Handle(Query message)
            {
                return await _db.Courses.Where(i => i.CourseID == message.Id).ProjectToSingleOrDefaultAsync<Model>();
            }
        }
    }
}