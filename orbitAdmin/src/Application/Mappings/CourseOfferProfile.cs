using AutoMapper;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetActiveCourseOffer;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Mappings
{
    public class CourseOfferProfile : Profile
    {
        public CourseOfferProfile()
        {
            CreateMap<AddEditCourseOfferCommand, CourseOffer>().ReverseMap();
            CreateMap<GetAllCourseOffersResponse, CourseOffer>().ReverseMap();
            CreateMap<GetActiveCourseOfferResponse, CourseOffer>().ReverseMap();
            CreateMap<GetCourseOfferByIdResponse, CourseOffer>().ReverseMap();
        }
    }
}
