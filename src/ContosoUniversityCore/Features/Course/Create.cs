namespace ContosoUniversityCore.Features.Course
{
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using MediatR;
    using Domain;
    using Infrastructure;

    public class Create
    {
        public class Command : IRequest
        {
            [IgnoreMap]
            public int Number { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            public Department Department { get; set; }
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            protected override void HandleCore(Command message)
            {
                var course = Mapper.Map<Command, Course>(message);
                course.CourseID = message.Number;

                _db.Courses.Add(course);
            }
        }
    }
}