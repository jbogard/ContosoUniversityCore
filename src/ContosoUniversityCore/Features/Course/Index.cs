namespace ContosoUniversityCore.Features.Course
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Domain;
    using Infrastructure;
    using MediatR;

    public class Index
    {
        public class Query : IAsyncRequest<Result>
        {
            public Department SelectedDepartment { get; set; }
        }

        public class Result
        {
            public Department SelectedDepartment { get; set; }
            public List<Course> Courses { get; set; }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                public string DepartmentName { get; set; }
            }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<Result> Handle(Query message)
            {
                int? departmentID = message.SelectedDepartment?.DepartmentID;

                var courses = await _db.Courses
                    .Where(c => !departmentID.HasValue || c.DepartmentID == departmentID)
                    .OrderBy(d => d.Id)
                    .ProjectToListAsync<Result.Course>();

                return new Result
                {
                    Courses = courses,
                    SelectedDepartment = message.SelectedDepartment
                };
            }
        }
    }
}