namespace ContosoUniversityCore.Features.Instructor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using FluentValidation;
    using Infrastructure;
    using MediatR;
    using AutoMapper;

    public class CreateEdit
    {
        public class Query : IRequest<Command>
        {
            public int? Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }


        public class Command : IRequest<int>
        {
            public Command()
            {
                AssignedCourses = new List<AssignedCourseData>();
                CourseInstructors = new List<CourseInstructor>();
                SelectedCourses = new string[0];
            }

            public int? Id { get; set; }

            public string LastName { get; set; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            public DateTime? HireDate { get; set; }

            [Display(Name = "Location")]
            public string OfficeAssignmentLocation { get; set; }

            [IgnoreMap]
            public string[] SelectedCourses { get; set; }

            [IgnoreMap]
            public List<AssignedCourseData> AssignedCourses { get; set; }
            public List<CourseInstructor> CourseInstructors { get; set; }

            public class AssignedCourseData
            {
                public int CourseID { get; set; }
                public string Title { get; set; }
                public bool Assigned { get; set; }
            }

            public class CourseInstructor
            {
                public int CourseID { get; set; }
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.LastName).NotNull().Length(0, 50);
                RuleFor(m => m.FirstMidName).NotNull().Length(0, 50);
                RuleFor(m => m.HireDate).NotNull();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext _db;

            public QueryHandler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<Command> Handle(Query message)
            {
                Command model;
                if (message.Id == null)
                {
                    model = new Command();
                }
                else
                {
                    model = await _db.Instructors
                        .Where(i => i.Id == message.Id)
                        .ProjectToSingleOrDefaultAsync<Command>();
                }

                PopulateAssignedCourseData(model);

                return model;
            }

            private void PopulateAssignedCourseData(Command model)
            {
                var allCourses = _db.Courses;
                var instructorCourses = new HashSet<int>(model.CourseInstructors.Select(c => c.CourseID));
                var viewModel = allCourses.Select(course => new Command.AssignedCourseData
                {
                    CourseID = course.Id,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.Id)
                }).ToList();
                model.AssignedCourses = viewModel;
            }

        }

        public class CommandHandler : IAsyncRequestHandler<Command, int>
        {
            private readonly SchoolContext _db;

            public CommandHandler(SchoolContext db)
            {
                _db = db;
            }

            public async Task<int> Handle(Command message)
            {
                Instructor instructor;
                if (message.Id == null)
                {
                    instructor = new Instructor();
                    _db.Instructors.Add(instructor);
                }
                else
                {
                    instructor = await _db.Instructors
                        .Include(i => i.OfficeAssignment)
                        .Include(i => i.CourseInstructors)
                        .Where(i => i.Id == message.Id)
                        .SingleAsync();
                }

                var courses = await _db.Courses.ToListAsync();

                instructor.Handle(message, courses);

                await _db.SaveChangesAsync();

                return instructor.Id;
            }
        }
    }
}