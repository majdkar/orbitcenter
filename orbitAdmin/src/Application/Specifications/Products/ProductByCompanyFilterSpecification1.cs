using System;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductByCompanyFilterSpecification1 : HeroSpecification<Product>
    {
        public ProductByCompanyFilterSpecification1(string searchString, string nameEn = "", int productParentCategoryId = 0, int productSubCategoryId = 0, int productSubSubCategoryId = 0, int productSubSubSubCategoryId = 0,  decimal retailpricestart = 0, decimal retailpriceend = 0)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString)) &&
                                 !p.Deleted ||


                                 ((String.IsNullOrEmpty(nameEn) ? p.NameEn.Length > 0 : p.NameEn == nameEn) &&
               (productParentCategoryId == 0 ? (p.ProductParentCategoryId == null || p.ProductParentCategoryId > 0) : p.ProductParentCategoryId == productParentCategoryId) &&
                   (productSubCategoryId == 0 ? (p.ProductSubCategoryId == null || p.ProductSubCategoryId > 0) : p.ProductSubCategoryId == productSubCategoryId) &&
                   (productSubSubCategoryId == 0 ? (p.ProductSubSubCategoryId == null || p.ProductSubSubCategoryId > 0) : p.ProductSubSubCategoryId == productSubSubCategoryId) &&
                   (productSubSubSubCategoryId == 0 ? (p.ProductSubSubCategoryId == null || p.ProductSubSubCategoryId > 0) : p.ProductSubSubCategoryId == productSubSubSubCategoryId));

            }
            else
            {
                Criteria = p => !p.Deleted;


            }
        }
    }
}