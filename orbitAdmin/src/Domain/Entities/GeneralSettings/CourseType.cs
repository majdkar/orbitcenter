using SchoolV01.Domain.Contracts;
using System.Collections.Generic;

namespace SchoolV01.Domain.Entities.GeneralSettings
{
    public class CourseType : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
     
    }
}