using AutoMapper;
using SchoolV01.Application.Features.CourseTypes.Commands;
using SchoolV01.Application.Features.CourseTypes.Queries;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Mappings
{
    public class CourseTypeProfile : Profile
    {
        public CourseTypeProfile()
        {
            CreateMap<AddEditCourseTypeCommand, CourseType>().ReverseMap();
            CreateMap<GetAllCourseTypesResponse, CourseType>().ReverseMap();
        }
    }
}
