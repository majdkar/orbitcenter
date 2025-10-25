using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SchoolV01.Domain.Entities.Orders
{
    public class ProductOrder : AuditableEntity<int>
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }


        public string Status { get; set; } 
        public string ClientType { get; set; } 

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string  PaymentStatus { get; set; }
        public string  OrderNumber { get; set; }

        // المجموع الكلي للطلب
        public decimal TotalPrice { get; set; }

        public int? PayTypeId { get; set; }
        public PayType PayType { get; set; }

        public string PaymentTransactionNumber { get; set; }

        // كل العناصر (المنتجات) داخل الطلب
        public ICollection<ProductOrderItem> Items { get; set; } = new List<ProductOrderItem>();

    }
}
