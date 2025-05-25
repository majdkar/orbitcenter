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
    public class PersonFilterSpecification : HeroSpecification<Person>
    {
        public PersonFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.Deleted;
            }
            else
            {
                Criteria = p => !p.Deleted;
            }
        }
    }
}
