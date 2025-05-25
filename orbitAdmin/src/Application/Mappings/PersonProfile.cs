using AutoMapper;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;

using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Shared.Constants.Role;

namespace SchoolV01.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<AddEditPersonCommand, Person>().ReverseMap();


            CreateMap<AddEditPersonCommand, BlazorHeroUser>()
                                 .ForMember(c => c.ClientType, opt => opt.MapFrom(src => RoleConstants.BasicRole));

            CreateMap<GetAllPersonsResponse, Person>().ReverseMap();
            CreateMap<GetAllPersonsResponse, Person>().ReverseMap();
        }
    }
}
