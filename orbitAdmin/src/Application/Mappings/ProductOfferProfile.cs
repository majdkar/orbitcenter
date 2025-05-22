using AutoMapper;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetActiveProductOffer;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Mappings
{
    public class ProductOfferProfile : Profile
    {
        public ProductOfferProfile()
        {
            CreateMap<AddEditProductOfferCommand, ProductOffer>().ReverseMap();
            CreateMap<GetAllProductOffersResponse, ProductOffer>().ReverseMap();
            CreateMap<GetActiveProductOfferResponse, ProductOffer>().ReverseMap();
            CreateMap<GetProductOfferByIdResponse, ProductOffer>().ReverseMap();
        }
    }
}
