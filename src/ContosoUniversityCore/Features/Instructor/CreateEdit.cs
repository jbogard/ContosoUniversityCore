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
        public class Query : IAsyncRequest<Command>
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


        public class Command : IAsyncRequest
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

            public string[] SelectedCourses { get; set; }

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
                        .Where(i => i.ID == message.Id)
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
                    CourseID = course.CourseID,
                    Title = course.Title,
                    Assigned = instructorCourses.Contains(course.CourseID)
                }).ToList();
                model.AssignedCourses = viewModel;
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
                        .Where(i => i.ID == message.Id)
                        .SingleAsync();
                }
                instructor.FirstMidName = message.FirstMidName;
                instructor.LastName = message.LastName;
                instructor.HireDate = message.HireDate.Value;

                if (String.IsNullOrWhiteSpace(message.OfficeAssignmentLocation))
                {
                    instructor.OfficeAssignment = null;
                }
                else if (instructor.OfficeAssignment == null)
                {
                    instructor.OfficeAssignment = new OfficeAssignment
                    {
                        Location = message.OfficeAssignmentLocation
                    };
                }
                else
                {
                    instructor.OfficeAssignment.Location = message.OfficeAssignmentLocation;
                }

                UpdateInstructorCourses(message.SelectedCourses, instructor);

            }

            private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
            {
                if (selectedCourses == null)
                {
                    instructorToUpdate.CourseInstructors = new List<CourseInstructor>();
                    return;
                }

                var selectedCoursesHS = new HashSet<string>(selectedCourses);
                var instructorCourses = new HashSet<int>
                    (instructorToUpdate.CourseInstructors.Select(c => c.CourseID));
                foreach (var course in _db.Courses)
                {
                    if (selectedCoursesHS.Contains(course.CourseID.ToString()))
                    {
                        if (!instructorCourses.Contains(course.CourseID))
                        {
                            instructorToUpdate.CourseInstructors.Add(new CourseInstructor { Course = course, Instructor = instructorToUpdate});
                        }
                    }
                    else
                    {
                        if (instructorCourses.Contains(course.CourseID))
                        {
                            var toRemove = instructorToUpdate.CourseInstructors.Where(ci => ci.CourseID == course.CourseID).Single();
                            instructorToUpdate.CourseInstructors.Remove(toRemove);
                        }
                    }
                }
            }

        }
    }
}