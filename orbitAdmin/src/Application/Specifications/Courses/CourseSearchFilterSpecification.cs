using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class CourseSearchFilterSpecification : HeroSpecification<Course>
    {
        public CourseSearchFilterSpecification(string searchString, string Coursename, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid,int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            //Includes.Add(p => p.CourseDefaultCategoryId);
            //Includes.Add(p => p.CourseParentCategoryId);
            //Includes.Add(p => p.CourseRatings);
            //IncludeStrings.Add("CourseRatings.Client");

            //Criteria = p => !p.IsDeleted;
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.Deleted &&
                                 ((Coursename == null ? p.NameEn.Length > 0 : p.NameEn.Contains(Coursename)) ||
                                 (Coursename == null ? p.NameAr.Length > 0 : p.NameAr.Contains(Coursename))) &&
                                (propductcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseParentCategoryId == propductcategoryid) &&
                                (propductSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubCategoryId == propductSubcategoryid) &&
                                (propductSubSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubSubCategoryId == propductSubSubcategoryid) &&
                                (propductSubSubSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubSubSubCategoryId == propductSubSubSubcategoryid) &&
                                //(fromprice == 0 ? p.Price > 0 : p.Price >= fromprice) &&
                                //(toprice == 0 ? p.Price > 0 : p.Price <= toprice) &&
                                (p.NameAr.Contains(searchString) &&
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAr1.Contains(searchString) ||
                                p.DescriptionEn1.Contains(searchString) ||
                                //p.Brand.Name.Contains(searchString) ||
                                p.Code.Contains(searchString) ||
                                //p.Price.ToString().Contains(searchString) ||
                                p.CourseDefaultCategory.NameEn.Contains(searchString) ||
                                p.CourseDefaultCategory.NameAr.Contains(searchString) ||
                                p.CourseParentCategory.NameAr.Contains(searchString) ||
                                p.CourseParentCategory.NameEn.Contains(searchString));
            }
            else
            {
                Criteria = p => !p.Deleted && ((Coursename == null ? p.NameEn.Length > 0 : p.NameEn.ToLower().Contains(Coursename.ToLower())) || Coursename == null ? p.NameAr.Length > 0 : p.NameAr.ToLower().Contains(Coursename.ToLower())) &&
                                (propductcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseParentCategoryId == propductcategoryid) &&
                                           (propductSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubCategoryId == propductSubcategoryid) &&
                                (propductSubSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubSubCategoryId == propductSubSubcategoryid) &&
                                (propductSubSubSubcategoryid == 0 ? p.CourseDefaultCategoryId > 0 : p.CourseSubSubSubCategoryId == propductSubSubSubcategoryid);
                                //(fromprice == 0 ? p.Price >= 0 : p.Price >= fromprice) &&
                                //(toprice == 0 ? p.Price >= 0 : p.Price <= toprice);
            }
        }
    }
}