using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Specifications.GeneralSettings
{
    public class PayTypeFilterSpecification : HeroSpecification<PayType>
    {
        public PayTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) || p.NameEn.Contains(searchString)) && !p.Deleted;
            }
            else
            {
                Criteria = p => !p.Deleted;
            }
        }
    }
}
