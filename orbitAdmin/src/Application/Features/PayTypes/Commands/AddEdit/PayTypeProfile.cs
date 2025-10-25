using AutoMapper;
using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Mappings
{
    public class PayTypeProfile : Profile
    {
        public PayTypeProfile()
        {
            CreateMap<AddEditPayTypeCommand, PayType>().ReverseMap();
            CreateMap<GetAllPayTypesResponse, PayType>().ReverseMap();
        }
    }
}
