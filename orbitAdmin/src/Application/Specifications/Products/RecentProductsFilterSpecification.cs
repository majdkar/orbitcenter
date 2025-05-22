using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;


namespace SchoolV01.Application.Specifications.Products
{
    public class RecentProductsFilterSpecification : HeroSpecification<Product>
    {
        public RecentProductsFilterSpecification(string searchString)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.Deleted &&
                                p.IsRecent &&
                                (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAr1.Contains(searchString) ||   
                                p.DescriptionEn1.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString));
            }
            else
            {
                Criteria = p => !p.Deleted && p.IsRecent;
            }

        }
    }
}
