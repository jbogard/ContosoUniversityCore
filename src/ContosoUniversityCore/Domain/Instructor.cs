using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityCore.Domain
{
    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public virtual ICollection<CourseInstructor> CourseInstructors { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }
    }

    public class CourseInstructor
    {
        public virtual int CourseID { get; set; }
        public virtual int InstructorID { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }

}