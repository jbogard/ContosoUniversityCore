namespace ContosoUniversityCore.Features.Course
{
    using AutoMapper;
    using Domain;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, Index.Result.Course>();
            CreateMap<Course, Details.Model>();
            CreateMap<Create.Command, Course>(MemberList.Source);
            CreateMap<Course, Edit.Command>().ReverseMap();
            CreateMap<Course, Delete.Command>();
        }
    }
}