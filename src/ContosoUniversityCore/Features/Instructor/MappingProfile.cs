namespace ContosoUniversityCore.Features.Instructor
{
    using AutoMapper;
    using Domain;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Instructor, Index.Model.Instructor>();
            CreateMap<CourseInstructor, Index.Model.CourseInstructor>();
            CreateMap<Course, Index.Model.Course>();
            CreateMap<Enrollment, Index.Model.Enrollment>();

            CreateMap<Instructor, CreateEdit.Command>();
            CreateMap<CourseInstructor, CreateEdit.Command.CourseInstructor>();

            CreateMap<Instructor, Details.Model>();
            CreateMap<Instructor, Delete.Command>();
        }
    }
}