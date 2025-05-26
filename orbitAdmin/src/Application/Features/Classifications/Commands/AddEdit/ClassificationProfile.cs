using AutoMapper;
using SchoolV01.Application.Features.Classifications.Commands;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Mappings
{
    public class ClassificationProfile : Profile
    {
        public ClassificationProfile()
        {
            CreateMap<AddEditClassificationCommand, Classification>().ReverseMap();
            CreateMap<GetAllClassificationsResponse, Classification>().ReverseMap();
        }
    }
}
