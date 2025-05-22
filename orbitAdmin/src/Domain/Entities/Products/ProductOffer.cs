using SchoolV01.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Entities.Products
{
    public class ProductOffer : AuditableEntity<int>
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
