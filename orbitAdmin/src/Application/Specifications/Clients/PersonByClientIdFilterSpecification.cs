using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Clients
{
    public class PersonByClientIdFilterSpecification : HeroSpecification<Person>
    {
        public PersonByClientIdFilterSpecification(int clientId)
        {
            Includes.Add(c => c.Client);
 

            Criteria = c => !c.Deleted&& c.ClientId == clientId;
        }
    }
}
