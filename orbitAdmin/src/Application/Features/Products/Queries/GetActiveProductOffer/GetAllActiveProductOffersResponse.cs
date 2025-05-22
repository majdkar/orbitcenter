using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetActiveProductOffer
{
    public class GetAllActiveProductOffersResponse
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string? ProductNameAr { get; set; }
        public string? ProductNameEn { get; set; }
        public string? ProductImageUrl { get; set; }



        public decimal? DiscountRatio { get; set; }

        public decimal? Price { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? OldPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
