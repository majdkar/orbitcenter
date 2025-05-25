using SchoolV01.Application.Features.Clients.Companies.Queries.GetPendingCompanies;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Clients
{
    public class PendingCompaniesFilterSpecification : HeroSpecification<Company>
    {
        public PendingCompaniesFilterSpecification( GetPendingCompaniesQuery request)
        {
            Includes.Add(p => p.Client);
           

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                Criteria = p => ((string.IsNullOrEmpty(request.CompanyName) ? true : p.NameAr.Contains(request.CompanyName)) ||
                    (string.IsNullOrEmpty(request.CompanyName) ? true : p.NameEn.Contains(request.CompanyName))) &&
                    (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                     (request.CountryId == 0 ? true : p.CountryId == request.CountryId) &&
                    (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) && (p.NameAr.Contains(request.SearchString) ||
                    p.NameEn.Contains(request.SearchString) ||
                    p.Address.Contains(request.SearchString) ||
                    p.Email.Contains(request.SearchString) ||
                    p.Phone.Contains(request.SearchString) ||
                    p.Website.Contains(request.SearchString)) &&
                    p.Client.Status == ClientStatusEnum.Pending.ToString() &&
                    !p.Deleted;
            }
            else
            {
                Criteria = p => ((string.IsNullOrEmpty(request.CompanyName) ? true : p.NameAr.Contains(request.CompanyName)) ||
                    (string.IsNullOrEmpty(request.CompanyName) ? true : p.NameEn.Contains(request.CompanyName))) &&
                    (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                    (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                     (request.CountryId == 0 ? true : p.CountryId == request.CountryId) &&
                    p.Client.Status == ClientStatusEnum.Pending.ToString() &&
                    !p.Deleted;
            }
        }
    }
}
