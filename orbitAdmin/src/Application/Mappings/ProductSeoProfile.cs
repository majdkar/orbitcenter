using AutoMapper;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Mappings
{
    public class ProductSeoProfile : Profile
    {
        public ProductSeoProfile()
        {
            CreateMap<AddEditProductSeoCommand, ProductSeo>().ReverseMap();
            CreateMap<GetAllProductSeosResponse, ProductSeo>().ReverseMap();
            CreateMap<GetProductSeoByIdResponse, ProductSeo>().ReverseMap();
        }
    }
}
