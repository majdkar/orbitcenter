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
    public class PersonByIdFilterSpecification : HeroSpecification<Person>
    {
        public PersonByIdFilterSpecification(int id)
        {
            Includes.Add(c => c.Client);
            Includes.Add(c => c.Country);
            Includes.Add(c => c.City);
            Criteria = c => c.Id == id;
        }
    }
}
