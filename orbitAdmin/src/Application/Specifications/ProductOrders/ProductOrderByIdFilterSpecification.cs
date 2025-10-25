using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductOrderByIdFilterSpecification : HeroSpecification<ProductOrder>
    {
        public ProductOrderByIdFilterSpecification(int id)
        {
            Includes.Add(x => x.PayType);

            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");
            IncludeStrings.Add("Items.Product"); // مهم: تضمين المنتجات عبر العناصر


            Criteria = x => x.Id == id;
        }
    }
}
