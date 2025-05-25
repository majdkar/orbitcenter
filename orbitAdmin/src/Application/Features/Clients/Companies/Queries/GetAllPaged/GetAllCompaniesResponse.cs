using SchoolV01.Domain.Entities.GeneralSettings;
using System;
using SchoolV01.Domain.Entities.Clients;

using Newtonsoft.Json;


namespace SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged
{
    public class GetAllCompaniesResponse
    {
        public int Id { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public int ClientId { get; set; }

        [JsonIgnore]
        public virtual Client Client { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string CityName { get; set; }
        public int? CityId { get; set; }
        public virtual City City { get; set; }
   
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public string CompanyImageUrl { get; set; }
        public string CompanyFileUrl { get; set; }
        public DateTime? LicenseIssuingDate { get; set; }

        public string ResponsiblePersonNameAr { get; set; }
        public string ResponsiblePersonNameEn { get; set; }
        public string ResponsiblePersonMobile { get; set; }

        public string AdditionalInfo { get; set; }

        public string UserId { get; set; }
        public string Status {  get; set; }
        

    }
}
