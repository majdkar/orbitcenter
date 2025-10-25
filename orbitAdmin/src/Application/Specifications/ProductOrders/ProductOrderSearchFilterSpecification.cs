using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Orders;
using System.Linq;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductOrderSearchFilterSpecification : HeroSpecification<ProductOrder>
    {
      
            public ProductOrderSearchFilterSpecification(
                string searchString,
                string orderNumber,
                int productId,
                int clientId,
                decimal fromPrice,
                decimal toPrice)
            {
            Includes.Add(x => x.PayType);

            Includes.Add(x => x.Client);
                IncludeStrings.Add("Client.Person");
                IncludeStrings.Add("Client.Company");
                IncludeStrings.Add("Items.Product"); // تضمين المنتجات داخل عناصر الطلب

                Criteria = p =>
                    !p.Deleted &&

                    // 🔍 البحث النصي العام (على رقم الطلب أو اسم المنتج)
                    (string.IsNullOrEmpty(searchString) ||
                        p.OrderNumber.Contains(searchString) ||
                        p.Items.Any(i =>
                            i.Product.NameAr.Contains(searchString) ||
                            i.Product.NameEn.Contains(searchString)
                        )
                    ) &&

                    // 🔍 البحث حسب رقم الطلب
                    (string.IsNullOrEmpty(orderNumber) || p.OrderNumber.Contains(orderNumber)) &&

                    // 🔍 البحث حسب المنتج (داخل العناصر)
                    (productId == 0 || p.Items.Any(i => i.ProductId == productId)) &&

                    // 🔍 البحث حسب العميل
                    (clientId == 0 || p.ClientId == clientId) &&

                    // 🔍 التصفية حسب السعر (مجموع الطلب)
                    (fromPrice == 0 || p.TotalPrice >= fromPrice) &&
                    (toPrice == 0 || p.TotalPrice <= toPrice);
            }
        }
    }
