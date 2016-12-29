using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityCore.Domain
{
    using Features.Instructor;
    using System.Linq;

    public class Instructor : Person
    {
        public DateTime HireDate { get; private set; }

        public virtual ICollection<CourseInstructor> CourseInstructors
            { get; private set; } = new List<CourseInstructor>();

        public virtual OfficeAssignment OfficeAssignment { get; private set; }

        public void Handle(CreateEdit.Command message, 
            IEnumerable<Course> courses)
        {
            UpdateDetails(message);

            UpdateInstructorCourses(message.SelectedCourses, courses);
        }

        public void Handle(Delete.Command message) => OfficeAssignment = null;

        private void UpdateDetails(CreateEdit.Command message)
        {
            FirstMidName = message.FirstMidName;
            LastName = message.LastName;
            HireDate = message.HireDate.GetValueOrDefault();

            if (string.IsNullOrWhiteSpace(message.OfficeAssignmentLocation))
            {
                OfficeAssignment = null;
            }
            else if (OfficeAssignment == null)
            {
                OfficeAssignment = new OfficeAssignment
                {
                    Location = message.OfficeAssignmentLocation
                };
            }
            else
            {
                OfficeAssignment.Location = message.OfficeAssignmentLocation;
            }
        }

        private void UpdateInstructorCourses(string[] selectedCourses, IEnumerable<Course> courses)
        {
            if (selectedCourses == null)
            {
                CourseInstructors = new List<CourseInstructor>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedCourses);
            var instructorCourses = new HashSet<int>
                (CourseInstructors.Select(c => c.CourseID));

            foreach (var course in courses)
            {
                if (selectedCoursesHS.Contains(course.Id.ToString()))
                {
                    if (!instructorCourses.Contains(course.Id))
                    {
                        CourseInstructors.Add(new CourseInstructor { Course = course, Instructor = this});
                    }
                }
                else
                {
                    if (instructorCourses.Contains(course.Id))
                    {
                        var toRemove = this.CourseInstructors.Single(ci => ci.CourseID == course.Id);
                        CourseInstructors.Remove(toRemove);
                    }
                }
            }
        }
    }

    public class CourseInstructor
    {
        public virtual int CourseID { get; set; }
        public virtual int InstructorID { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }

}