namespace ContosoUniversityCore.Features.Student
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using FluentValidation;
    using Infrastructure;
    using MediatR;
    using Domain;

    public class Create
    {
        public class Command : IRequest
        {
            public string LastName { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            public DateTime? EnrollmentDate { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.LastName).NotNull().Length(1, 50);
                RuleFor(m => m.FirstMidName).NotNull().Length(1, 50);
                RuleFor(m => m.EnrollmentDate).NotNull();
            }
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
                var student = Mapper.Map<Command, Student>(message);

                _db.Students.Add(student);
            }
        }
    }
}