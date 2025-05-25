using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Entities.Clients
{
    public class Company : AuditableEntity<int>
    {
        public string NameAr { get; set; }

        public string NameEn { get; set; }

        [ForeignKey("Clinet")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public  bool IsFeatured { get; set; }
        public string? CityName { get; set; }
        [ForeignKey("City")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string GeoLocation {  get; set; }
        public string CompanyImageUrl { get; set; }
        public string CompanyFileUrl { get; set; }
        public DateTime? LicenseIssuingDate { get; set; }

        public string ResponsiblePersonNameAr { get; set; }
        public string ResponsiblePersonNameEn { get; set; }
        public string ResponsiblePersonMobile { get; set; }

        public string? AdditionalInfo { get; set; }

        
    }
}
