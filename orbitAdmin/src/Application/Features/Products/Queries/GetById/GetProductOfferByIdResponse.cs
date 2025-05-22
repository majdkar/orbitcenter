using System;


namespace SchoolV01.Application.Features.Products.Queries.GetById
{
    public class GetProductOfferByIdResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
