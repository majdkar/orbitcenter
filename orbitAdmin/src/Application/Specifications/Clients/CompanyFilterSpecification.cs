using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Clients;


namespace SchoolV01.Application.Specifications.Clients
{
    public class CompanyFilterSpecification : HeroSpecification<Company>
    {
        public CompanyFilterSpecification()
        {
            Criteria = p => !p.Deleted && p.Client.Status == ClientStatusEnum.Accepted.ToString();
        }
    }
}
