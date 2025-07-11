using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProductSeoByIdFilterSpecification : HeroSpecification<ProductSeo>
    {
        public ProductSeoByIdFilterSpecification(int id)
        {
            Includes.Add(x => x.Product);
      

            Criteria = x => x.Id == id;
        }
    }
}
