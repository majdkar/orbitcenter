using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Orders;
using System.Linq;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductOrderByClientFilterSpecification : HeroSpecification<ProductOrder>
    {
        public ProductOrderByClientFilterSpecification(string searchString,int clientId)
        {
            Includes.Add(x => x.PayType);
            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");
            IncludeStrings.Add("Items.Product"); // مهم: تضمين المنتجات عبر العناصر

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>
                    !p.Deleted &&
                    (p.ClientId == clientId) &&
                    (
                        p.OrderNumber.Contains(searchString) ||
                        p.Items.Any(i =>
                            i.Product.NameAr.Contains(searchString) ||
                            i.Product.NameEn.Contains(searchString)
                        )
                    );
            }
            else
            {
                Criteria = p => !p.Deleted &&
                    (p.ClientId == clientId);
            }
        }
    }
}