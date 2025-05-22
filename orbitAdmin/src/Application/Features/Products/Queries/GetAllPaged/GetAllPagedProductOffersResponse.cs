using Newtonsoft.Json;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Products;
using System;


namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductOffersResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        [JsonProperty("ProductNameAr")]
        public string NameAr { get; set; }
        [JsonProperty("ProductNameEn")]
        public string NameEn { get; set; }
        [JsonProperty("ProductDescriptionAr")]
        public string? DescriptionAr4 { get; set; }
        public string? DescriptionAr1 { get; set; }
        public string? DescriptionAr2 { get; set; }
        public string? DescriptionAr3 { get; set; }
        [JsonProperty("ProductDescriptionEn")]
        public string? DescriptionEn1 { get; set; }
        public string? DescriptionEn2 { get; set; }
        public string? DescriptionEn3 { get; set; }
        public string? DescriptionEn4 { get; set; }


        [JsonProperty("ProductDescriptionGe")]
        public string? DescriptionGe1 { get; set; }
        public string? DescriptionGe2 { get; set; }
        public string? DescriptionGe3 { get; set; }
        public string? DescriptionGe4 { get; set; }


        [JsonProperty("ProductCode")]
        public string? Code { get; set; }

        [JsonProperty("Plan")]
        public string? Plan { get; set; }


        [JsonProperty("ProductCategoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("ProductCategory")]
        public virtual ProductCategory Category { get; set; }
        [JsonProperty("ProductParentCategoryId")]
        public int? ParentCategoryId { get; set; }
        [JsonProperty("ProductParentCategory")]
        public virtual ProductCategory ParentCategory { get; set; }




        [JsonProperty("ProductPrice")]
        public decimal? Price { get; set; }
        [JsonProperty("ProductOrder")]
        public int Order { get; set; } = 0;
        [JsonProperty("ProductIsVisible")]
        public bool IsVisible { get; set; } = true;
        [JsonProperty("ProductIsRecent")]
        public bool IsRecent { get; set; } = false;


        [JsonProperty("ProductProductImageUrl")]
        public string ProductImageUrl1 { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
