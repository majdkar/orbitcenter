using AutoMapper;

using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Application.Features.CourseOrders.Queries.GetById;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAll;

namespace SchoolV01.Application.Mappings
{
    public class CourseOrdersProfile : Profile
    {
        public CourseOrdersProfile()
        {
            CreateMap<AddEditCourseOrderCommand, CourseOrder>().ReverseMap();
            CreateMap<GetCourseOrderByIdResponse, CourseOrder>().ReverseMap();
            CreateMap<GetAllCourseOrdersResponse, CourseOrder>().ReverseMap(); 
        }
    }
}