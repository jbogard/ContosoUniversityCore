namespace ContosoUniversityCore.Features.Instructor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Infrastructure;
    using MediatR;
    using AutoMapper;

    public class Index
    {
        public class Query : IAsyncRequest<Model>
        {
            public int? Id { get; set; }
            public int? CourseID { get; set; }
        }

        public class Model
        {
            public int? InstructorID { get; set; }
            public int? CourseID { get; set; }

            public List<Instructor> Instructors { get; set; }
            public List<Course> Courses { get; set; }
            public List<Enrollment> Enrollments { get; set; }

            public class Instructor
            {
                public int ID { get; set; }

                [Display(Name = "Last Name")]
                public string LastName { get; set; }

                [Display(Name = "First Name")]
                public string FirstMidName { get; set; }

                [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
                [Display(Name = "Hire Date")]
                public DateTime HireDate { get; set; }

                public string OfficeAssignmentLocation { get; set; }

                public IEnumerable<CourseInstructor> CourseInstructors { get; set; }
            }

            public class CourseInstructor
            {
                public int CourseID { get; set; }
                public string CourseTitle { get; set; }
            }

            public class Course
            {
                public int Id { get; set; }
                public string Title { get; set; }
                public string DepartmentName { get; set; }
            }

            public class Enrollment
            {
                [DisplayFormat(NullDisplayText = "No grade")]
                public Grade? Grade { get; set; }
                public string StudentFullName { get; set; }
            }
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
                var instructors = await _db.Instructors
                    .OrderBy(i => i.LastName)
                    .ProjectToListAsync<Model.Instructor>();

                var courses = new List<Model.Course>();
                var enrollments = new List<Model.Enrollment>();

                if (message.Id != null)
                {
                    courses = await _db.CourseInstructors
                        .Where(ci => ci.InstructorID == message.Id)
                        .Select(ci => ci.Course)
                        .ProjectToListAsync<Model.Course>();
                }

                if (message.CourseID != null)
                {
                    enrollments = await _db.Enrollments
                        .Where(x => x.CourseID == message.CourseID)
                        .ProjectToListAsync<Model.Enrollment>();
                }

                var viewModel = new Model
                {
                    Instructors = instructors,
                    Courses = courses,
                    Enrollments = enrollments,
                    InstructorID = message.Id,
                    CourseID = message.CourseID
                };

                return viewModel;
            }
        }

    }
}