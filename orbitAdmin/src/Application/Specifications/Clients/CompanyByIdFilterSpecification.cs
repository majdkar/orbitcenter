using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Clients;

namespace SchoolV01.Application.Specifications.Clients
{
    public class CompanyByIdFilterSpecification : HeroSpecification<Company>
    {
        public CompanyByIdFilterSpecification(int id)
        {
            Includes.Add(c => c.Client);
    ;

            Criteria = c => c.Id == id;
        }
    }
}
