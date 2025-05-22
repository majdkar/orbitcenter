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
    public class MainProductCategoryFilterSpecification : HeroSpecification<ProductCategory>
    {
        public MainProductCategoryFilterSpecification(string searchString)
        {
            
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  (p.NameAr.Contains(searchString) || 
                p.NameEn.Contains(searchString) || 
                p.DescriptionAr1.Contains(searchString) || 
                p.DescriptionEn1.Contains(searchString)) 
                && (p.ParentCategoryId == null || p.ParentCategoryId == 0)
                && !p.Deleted;
            }
            else
            {
                Criteria = p => !p.Deleted && (p.ParentCategoryId == null || p.ParentCategoryId == 0);
            }
        }
    }
}
