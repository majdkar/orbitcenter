using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class AllCourseByCategoryIdFilterSpecification : HeroSpecification<Course>
    {
        public AllCourseByCategoryIdFilterSpecification(string searchString,int categoryId)
        {
            Includes.Add(p => p.CourseDefaultCategory);


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.Deleted && p.CourseDefaultCategoryId == categoryId &&
                                (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAr1.Contains(searchString) ||
                                p.DescriptionEn1.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString)||
                                //p.Price.ToString().Contains(searchString)||
                                p.CourseDefaultCategory.NameEn.Contains(searchString)||
                                p.CourseDefaultCategory.NameAr.Contains(searchString)||
                                p.CourseParentCategory.NameAr.Contains(searchString)||
                                p.CourseParentCategory.ParentCategory.NameEn.Contains(searchString));
            }
            else
            {
                Criteria = p =>  !p.Deleted && p.CourseDefaultCategoryId == categoryId;
            }
        }
    }
}