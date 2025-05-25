using AutoMapper;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetPendingCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAcceptedCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetRefusedCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;

using SchoolV01.Domain.Entities.Clients;
using System.Linq;
using SchoolV01.Application.Features.Clients.Companies.Queries;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Shared.Constants.Role;

namespace SchoolV01.Application.Mappings
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<AddEditCompanyCommand, Company>().ReverseMap();

        

            CreateMap<AddEditCompanyCommand, BlazorHeroUser>()
                                 .ForMember(c => c.ClientType, opt => opt.MapFrom(src => RoleConstants.BasicRole));


            CreateMap<GetAllCompaniesResponse, Company>().ReverseMap();
        }
    }
}
