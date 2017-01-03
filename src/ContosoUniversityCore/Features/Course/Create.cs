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

        public class Handler : IRequestHandler<Command>
        {
            private readonly SchoolContext _db;

            public Handler(SchoolContext db)
            {
                _db = db;
            }

            public void Handle(Command message)
            {
                var course = Mapper.Map<Command, Course>(message);
                course.Id = message.Number;

                _db.Courses.Add(course);
            }
        }
    }
}