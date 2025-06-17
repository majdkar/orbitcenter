using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProductByNameFilterSpecification : HeroSpecification<Product>
    {
        public ProductByNameFilterSpecification(string Name)
        {
            Includes.Add(x => x.ProductDefaultCategory);
      

            Criteria = x => x.NameEn == Name || x.NameAr == Name || x.NameGe == Name;
        }
    }
}
