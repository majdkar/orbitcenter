using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;
using System;

namespace SchoolV01.Application.Specifications.Products
{
    public class AllActiveProductOfferFilterSpecification : HeroSpecification<ProductOffer>
    {
        public AllActiveProductOfferFilterSpecification()
        {
            Includes.Add(p => p.Product);
            //Includes.Add(p => p.ProductWeight);
            //Includes.Add(p => p.ProductOptionSize);
            //IncludeStrings.Add("Product.Company");
            //IncludeStrings.Add("Product.ProductImages");
            Criteria = p => p.StartDate <= DateTime.Now.Date && p.EndDate >= DateTime.Now.Date && !p.Deleted;
        }
    }
}
