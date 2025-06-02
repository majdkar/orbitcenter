using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Domain.Entities.Suggestions
{
    public class Suggestion : AuditableEntity<int>
    {

        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }
        public SuggestionType Type { get; set; }

        public string Description { get; set; }

        public string Reply { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }
}
