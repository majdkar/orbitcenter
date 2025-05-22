using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;
using static SchoolV01.Shared.Constants.Permission.Permissions;

namespace SchoolV01.Application.Specifications.Products
{
    public class ProductFilterSpecification : HeroSpecification<Product>
    {
        public ProductFilterSpecification()
        {
     
            Criteria = p =>  !p.Deleted;
        }
    }
}
