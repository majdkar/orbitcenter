using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Domain.Entities.Orders
{
    public class CourseOrder : AuditableEntity<int>
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string Status { get; set; }
        public string ClientType { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string  PaymentStatus { get; set; }
        public string  OrderNumber { get; set; }
        public int? PayTypeId { get; set; }
        public PayType PayType { get; set; }

        public string PaymentTransactionNumber { get; set; }

        public decimal Price { get; set; } 

    }
}
