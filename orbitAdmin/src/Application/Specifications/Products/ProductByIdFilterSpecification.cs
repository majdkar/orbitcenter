using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProductByIdFilterSpecification :HeroSpecification<Product>
    {
        public ProductByIdFilterSpecification(int id)
        {
            Includes.Add(x => x.ProductDefaultCategory);
      

            Criteria = x => x.Id == id;
        }
    }
}
