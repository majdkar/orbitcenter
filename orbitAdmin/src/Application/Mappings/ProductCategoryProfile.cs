using AutoMapper;
using SchoolV01.Application.Features.ProductCategories.Commands.AddEdit;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Core.Entities;


namespace SchoolV01.Application.Mappings
{
    public class ProductCategoryProfile:Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<AddEditProductCategoryCommand, ProductCategory>().ReverseMap();
            CreateMap<GetAllProductCategoriesResponse, ProductCategory>().ReverseMap();
            CreateMap<GetAllProductCategorySonsResponse, ProductCategory>().ReverseMap();
            CreateMap<GetProductCategoriesByIdResponse, ProductCategory>().ReverseMap();
        }
    }
}
