using System;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductOffersResponse
    {
        public int Id { get; set; }
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
   

        public decimal? NewPrice { get; set; }
        public decimal? DiscountRatio { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
