using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.GeneralSettings;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Entities.Clients
{
    public class Person : AuditableEntity<int>
    {

       

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        [ForeignKey("City")]
        public int? CityId { get; set; }
        public virtual City City { get; set; }

        public string Sex { get; set; }

        public string PersomImageUrl { get; set; }
        public string CvFileUrl { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile2 { get; set; }
        public string Qualification { get; set; }
        public string Job { get; set; }

        [ForeignKey("Classification")]
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
