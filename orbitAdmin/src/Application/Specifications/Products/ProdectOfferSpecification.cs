
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProdectOfferSpecification : HeroSpecification<ProductOffer>
    {
        public ProdectOfferSpecification(int productId,string searchString)
        {

         
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Product.NameAr.Contains(searchString) ||
                                 p.Product.NameEn.Contains(searchString) ||
                                 p.Product.DescriptionEn1.Contains(searchString) ||
                                 p.Product.DescriptionAr1.Contains(searchString)||
                                 //p.Product.Price.ToString().Contains(searchString)||
                                 p.Product.ProductDefaultCategory.NameAr.Contains(searchString)||
                                 p.Product.ProductDefaultCategory.NameEn.Contains(searchString)) &&
                                 p.ProductId == productId && !p.Deleted;
            }
            else
            {
                Criteria = p => p.ProductId == productId && !p.Deleted;
            }
        }
    }
}
