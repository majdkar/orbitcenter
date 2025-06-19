using AutoMapper;

using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Mappings
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<AddEditCompanyCourseCommand, Course>().ReverseMap();
            CreateMap<GetCourseByIdResponse, Course>().ReverseMap();
            CreateMap<GetAllCoursesResponse, Course>().ReverseMap(); 
        }
    }
}