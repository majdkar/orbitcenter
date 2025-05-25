using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Clients
{
    public class CompanyByClientIdFilterSpecification : HeroSpecification<Company>
    {
        public CompanyByClientIdFilterSpecification(int clientId)
        {
            Includes.Add(c => c.Client);


            Criteria = c => c.ClientId == clientId;
        }
    }
}
