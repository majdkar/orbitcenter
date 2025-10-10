using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Domain.Entities.Orders
{
    public class ProductOrderItem : AuditableEntity<int>
    {
        public int ProductOrderId { get; set; }
        public ProductOrder ProductOrder { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        // السعر الفرعي (الكمية × السعر)
        public decimal SubTotal => UnitPrice * Quantity;

    }
}
