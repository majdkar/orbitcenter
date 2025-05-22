using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductSearchFilterSpecification : HeroSpecification<Product>
    {
        public ProductSearchFilterSpecification(string searchString, string productname, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid,int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            //Includes.Add(p => p.ProductDefaultCategoryId);
            //Includes.Add(p => p.ProductParentCategoryId);
            //Includes.Add(p => p.ProductRatings);
            //IncludeStrings.Add("ProductRatings.Client");

            //Criteria = p => !p.IsDeleted;
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.Deleted &&
                                 ((productname == null ? p.NameEn.Length > 0 : p.NameEn.Contains(productname)) ||
                                 (productname == null ? p.NameAr.Length > 0 : p.NameAr.Contains(productname))) &&
                                (propductcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductParentCategoryId == propductcategoryid) &&
                                (propductSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubCategoryId == propductSubcategoryid) &&
                                (propductSubSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubSubCategoryId == propductSubSubcategoryid) &&
                                (propductSubSubSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubSubSubCategoryId == propductSubSubSubcategoryid) &&
                                //(fromprice == 0 ? p.Price > 0 : p.Price >= fromprice) &&
                                //(toprice == 0 ? p.Price > 0 : p.Price <= toprice) &&
                                (p.NameAr.Contains(searchString) &&
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAr1.Contains(searchString) ||
                                p.DescriptionEn1.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString) ||
                                //p.Price.ToString().Contains(searchString) ||
                                p.ProductDefaultCategory.NameEn.Contains(searchString) ||
                                p.ProductDefaultCategory.NameAr.Contains(searchString) ||
                                p.ProductParentCategory.NameAr.Contains(searchString) ||
                                p.ProductParentCategory.NameEn.Contains(searchString));
            }
            else
            {
                Criteria = p => !p.Deleted && ((productname == null ? p.NameEn.Length > 0 : p.NameEn.ToLower().Contains(productname.ToLower())) || productname == null ? p.NameAr.Length > 0 : p.NameAr.ToLower().Contains(productname.ToLower())) &&
                                (propductcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductParentCategoryId == propductcategoryid) &&
                                           (propductSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubCategoryId == propductSubcategoryid) &&
                                (propductSubSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubSubCategoryId == propductSubSubcategoryid) &&
                                (propductSubSubSubcategoryid == 0 ? p.ProductDefaultCategoryId > 0 : p.ProductSubSubSubCategoryId == propductSubSubSubcategoryid);
                                //(fromprice == 0 ? p.Price >= 0 : p.Price >= fromprice) &&
                                //(toprice == 0 ? p.Price >= 0 : p.Price <= toprice);
            }
        }
    }
}