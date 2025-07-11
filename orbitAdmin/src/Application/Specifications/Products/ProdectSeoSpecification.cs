
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProdectSeoSpecification : HeroSpecification<ProductSeo>
    {
        public ProdectSeoSpecification(int productId,string searchString)
        {

         
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>   p.ProductId == productId && !p.Deleted;
            }
            else
            {
                Criteria = p => p.ProductId == productId && !p.Deleted;
            }
        }
    }
}
