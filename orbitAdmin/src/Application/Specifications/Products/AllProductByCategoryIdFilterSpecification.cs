using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class AllProductByCategoryIdFilterSpecification : HeroSpecification<Product>
    {
        public AllProductByCategoryIdFilterSpecification(string searchString,int categoryId)
        {
            Includes.Add(p => p.ProductDefaultCategory);


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.Deleted && p.ProductDefaultCategoryId == categoryId &&
                                (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAr1.Contains(searchString) ||
                                p.DescriptionEn1.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString)||
                                //p.Price.ToString().Contains(searchString)||
                                p.ProductDefaultCategory.NameEn.Contains(searchString)||
                                p.ProductDefaultCategory.NameAr.Contains(searchString)||
                                p.ProductParentCategory.NameAr.Contains(searchString)||
                                p.ProductParentCategory.ParentCategory.NameEn.Contains(searchString));
            }
            else
            {
                Criteria = p =>  !p.Deleted && p.ProductDefaultCategoryId == categoryId;
            }
        }
    }
}