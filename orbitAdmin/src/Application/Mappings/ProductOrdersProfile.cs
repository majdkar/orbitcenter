using AutoMapper;

using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Application.Features.ProductOrders.Queries.GetById;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAll;

namespace SchoolV01.Application.Mappings
{
    public class ProductOrdersProfile : Profile
    {
        public ProductOrdersProfile()
        {
            CreateMap<AddEditProductOrderCommand, ProductOrder>().ReverseMap();
            CreateMap<GetProductOrderByIdResponse, ProductOrder>().ReverseMap();
            CreateMap<GetAllProductOrdersResponse, ProductOrder>().ReverseMap(); 
        }
    }
}