using AutoMapper;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Mappings
{
    public class CourseSeoProfile : Profile
    {
        public CourseSeoProfile()
        {
            CreateMap<AddEditCourseSeoCommand, CourseSeo>().ReverseMap();
            CreateMap<GetAllCourseSeosResponse, CourseSeo>().ReverseMap();
            CreateMap<GetCourseSeoByIdResponse, CourseSeo>().ReverseMap();
        }
    }
}
