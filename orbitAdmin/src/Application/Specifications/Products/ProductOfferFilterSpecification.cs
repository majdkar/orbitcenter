using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProductOfferFilterSpecification : HeroSpecification<ProductOffer>
    {
        public ProductOfferFilterSpecification(int productId)
        {
            Criteria = p => p.ProductId == productId && !p.Deleted;
        }
    }
}