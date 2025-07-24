using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Features.Products.Queries.GetById
{
    public class GetProductByIdResponse
    {
        public int Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }

        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }
        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public string Code { get; set; }

        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        [ForeignKey("ProductParentCategory")]
        public int? ProductParentCategoryId { get; set; }
        public virtual ProductCategory ProductParentCategory { get; set; }
        public string? ProductParentCategoryNameAr { get; set; }
        public string? ProductParentCategoryNameEn { get; set; }
        public string? ProductParentCategoryNameGe { get; set; }


        public int? ProductSubCategoryId { get; set; }
        public string? ProductSubCategoryNameAr { get; set; }
        public string? ProductSubCategoryNameEn { get; set; }
        public string? ProductSubCategoryNameGe { get; set; }



        public int? ProductSubSubCategoryId { get; set; }
        public string? ProductSubSubCategoryNameAr { get; set; }
        public string? ProductSubSubCategoryNameEn { get; set; }
        public string? ProductSubSubCategoryNameGe { get; set; }


        public int? ProductSubSubSubCategoryId { get; set; }
        public string? ProductSubSubSubCategoryNameAr { get; set; }
        public string? ProductSubSubSubCategoryNameEn { get; set; }
        public string? ProductSubSubSubCategoryNameGe { get; set; }

        [ForeignKey("ProductDefaultCategory")]
        public int? ProductDefaultCategoryId { get; set; }
        public virtual ProductCategory ProductDefaultCategory { get; set; }



        public decimal? Price { get; set; }
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool IsRecent { get; set; } = false;

        public string Plan { get; set; }

        public string ProductImageUrl1 { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }

        public virtual List<ProductOffer> ProductOffers { get; set; }
        public virtual ProductSeo ProductSeos { get; set; }

        public string Keywords { get; set; }
        public string SeoDescription { get; set; }
    }
}
