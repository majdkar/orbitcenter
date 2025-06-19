using System;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class CourseByCompanyFilterSpecification1 : HeroSpecification<Course>
    {
        public CourseByCompanyFilterSpecification1(string searchString, string nameEn = "", int CourseParentCategoryId = 0, int CourseSubCategoryId = 0, int CourseSubSubCategoryId = 0, int CourseSubSubSubCategoryId = 0,  decimal retailpricestart = 0, decimal retailpriceend = 0)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString)) &&
                                 !p.Deleted ||


                                 ((String.IsNullOrEmpty(nameEn) ? p.NameEn.Length > 0 : p.NameEn == nameEn) &&
               (CourseParentCategoryId == 0 ? (p.CourseParentCategoryId == null || p.CourseParentCategoryId > 0) : p.CourseParentCategoryId == CourseParentCategoryId) &&
                   (CourseSubCategoryId == 0 ? (p.CourseSubCategoryId == null || p.CourseSubCategoryId > 0) : p.CourseSubCategoryId == CourseSubCategoryId) &&
                   (CourseSubSubCategoryId == 0 ? (p.CourseSubSubCategoryId == null || p.CourseSubSubCategoryId > 0) : p.CourseSubSubCategoryId == CourseSubSubCategoryId) &&
                   (CourseSubSubSubCategoryId == 0 ? (p.CourseSubSubCategoryId == null || p.CourseSubSubCategoryId > 0) : p.CourseSubSubCategoryId == CourseSubSubSubCategoryId));

            }
            else
            {
                Criteria = p => !p.Deleted;


            }
        }
    }
}