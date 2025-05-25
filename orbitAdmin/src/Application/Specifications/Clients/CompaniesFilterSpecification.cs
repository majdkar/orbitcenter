using SchoolV01.Application.Features.Clients.Companies.Queries.GetAcceptedCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Clients;
using System;
using System.Linq;

public class CompaniesFilterSpecification : HeroSpecification<Company>
{
    public CompaniesFilterSpecification(GetAllPagedClientCompaniesQuery request)
    {
        Includes.Add(p => p.Client);
       

        if (!string.IsNullOrEmpty(request.SearchString))
        {
            Criteria = p => ((string.IsNullOrEmpty(request.CompanyName) ? true : p.NameAr.Contains(request.CompanyName)) ||
                (string.IsNullOrEmpty(request.CompanyName) ? true : p.NameEn.Contains(request.CompanyName))) &&
                (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                 (request.CountryId==0 ? true : p.CountryId==request.CountryId) &&
                (p.NameEn.Contains(request.SearchString) ||
                p.Address.Contains(request.SearchString) ||
                p.Email.Contains(request.SearchString) ||
                p.Phone.Contains(request.SearchString) ||
                p.Website.Contains(request.SearchString)) &&
                p.Client.Status == ClientStatusEnum.Accepted.ToString() &&
               !p.Deleted;
        }
        else
        {
            Criteria = p => ((string.IsNullOrEmpty(request.CompanyName) ? true : p.NameAr.Contains(request.CompanyName)) ||
                (string.IsNullOrEmpty(request.CompanyName) ? true : p.NameEn.Contains(request.CompanyName))) &&
                (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                (request.CountryId == 0 ? true : p.CountryId == request.CountryId) &&
                p.Client.Status == ClientStatusEnum.Accepted.ToString() &&
                !p.Deleted;
        }
    }
}
