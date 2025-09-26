using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchoolV01.Domain.Entities.Clients
{
    public class Client : AuditableEntity<int>
    {
        public string Type { set; get; } // Check Shared.Constants.Clients.ClientTypesEnum.cs (Person-Company)

        public string Status { get; set; }// Check Shared.Constants.Clients.ClientStatusEnum.cs (Pending-Accepted-Refused)

        public string UserId { get; set; }
        public BlazorHeroUser User { get; set; }

        public double DiscountRatio { get; set; } = 0;

        

        [JsonIgnore]
        public virtual Person Person { get; set; }

        [JsonIgnore]
        public virtual Company Company { get; set; } 
        
        public bool IsActive { get; set; } = true;
    }
}
