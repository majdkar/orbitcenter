using AutoMapper;
using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Core.Entities;


namespace SchoolV01.Application.Mappings
{
    public class CourseCategoryProfile:Profile
    {
        public CourseCategoryProfile()
        {
            CreateMap<AddEditCourseCategoryCommand, CourseCategory>().ReverseMap();
            CreateMap<GetAllCourseCategoriesResponse, CourseCategory>().ReverseMap();
            CreateMap<GetAllCourseCategorySonsResponse, CourseCategory>().ReverseMap();
            CreateMap<GetCourseCategoriesByIdResponse, CourseCategory>().ReverseMap();
        }
    }
}
