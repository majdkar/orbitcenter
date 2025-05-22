using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Products
{
    public class AllPagedActiveProductOffersFilterSpecification : HeroSpecification<ProductOffer>
    {
        public AllPagedActiveProductOffersFilterSpecification(string searchString)
        {
            

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.StartDate <= DateTime.Now.Date && p.EndDate >= DateTime.Now.Date && !p.Deleted;

            }
            else
            {
                Criteria = p => p.StartDate <= DateTime.Now.Date && p.EndDate >= DateTime.Now.Date && !p.Deleted;
            }

        }
    }
}
