using SchoolV01.Application.Features.Clients.Persons.Queries.GetAllPaged;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Clients
{
    internal class SearchPersonFilterSpecification : HeroSpecification<Person>
    {
        public SearchPersonFilterSpecification(GetAllPagedPersonsQuery request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                Criteria = p => 
                                (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                               
                                (string.IsNullOrEmpty(request.PersonName) ? true : p.FullName.Contains(request.PersonName)  ||
                                string.IsNullOrEmpty(request.PersonName) ? true : p.FullNameEn.Contains(request.PersonName)) &&
                                                                (string.IsNullOrEmpty(request.Status) ? true : p.Client.Status.Contains(request.Status)) &&

                                (
                                p.Email.Contains(request.SearchString) || p.Phone.Contains(request.PhoneNumber) || p.FullName.Contains(request.PersonName))
                              &&
                                !p.Deleted;
            }
            else
            {
                Criteria = p => 
                                (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                                (string.IsNullOrEmpty(request.PersonName) ? true : p.FullName.Contains(request.PersonName) ||
                                string.IsNullOrEmpty(request.PersonName) ? true : p.FullNameEn.Contains(request.PersonName)) &&
                                (string.IsNullOrEmpty(request.Status) ? true : p.Client.Status.Contains(request.Status)) &&

                                !p.Deleted;
            }
        }
    }
}
