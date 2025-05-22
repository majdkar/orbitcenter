using SchoolV01.Application.Specifications.Base;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class ProductCategoryFilterSpecification : HeroSpecification<ProductCategory>
    {
        public ProductCategoryFilterSpecification(string searchString)
        {
            
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  (p.NameAr.Contains(searchString) ||
                p.NameEn.Contains(searchString) || 
                p.DescriptionAr1.Contains(searchString) || 
                p.DescriptionEn1.Contains(searchString) || 
                p.ParentCategory.NameEn.Contains(searchString) || 
                p.ParentCategory.NameAr.Contains(searchString)) && 

                !p.Deleted;
            }
            else
            {
                Criteria = p => !p.Deleted;
            }
        }
    }
}
