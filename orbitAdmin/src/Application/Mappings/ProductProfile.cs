using AutoMapper;

using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditCompanyProductCommand, Product>().ReverseMap();
            CreateMap<GetProductByIdResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsResponse, Product>().ReverseMap(); 
        }
    }
}