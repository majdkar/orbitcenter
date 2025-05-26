
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.GeneralSettings;
using System;


namespace SchoolV01.Application.Features.Clients.Persons.Queries.GetAll
{
    public class GetAllPersonsResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; } 

        public string UserId { get; set; }
        public virtual Client Client { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public string Sex { get; set; }

        public string IdentifierImageUrl { get; set; }
        public string PersomImageUrl { get; set; }
        public string CvFileUrl { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Qualification { get; set; }
        public string Job { get; set; }

        public int? ClassificationId { get; set; }
        public virtual Classification Classification { get; set; }

        public string Fax { get; set; }
        public string MailBox { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }

        public string AdditionalInfo { get; set; }

    }
}
