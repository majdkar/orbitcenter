using SchoolV01.Domain.Contracts;

namespace SchoolV01.Domain.Entities.GeneralSettings
{
    public class PayType : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
