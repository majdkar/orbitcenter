
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseOfferSpecification : HeroSpecification<CourseOffer>
    {
        public CourseOfferSpecification(int CourseId,string searchString)
        {

         
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Course.NameAr.Contains(searchString) ||
                                 p.Course.NameEn.Contains(searchString) ||
                                 p.Course.DescriptionEn1.Contains(searchString) ||
                                 p.Course.DescriptionAr1.Contains(searchString)||
                                 //p.Course.Price.ToString().Contains(searchString)||
                                 p.Course.CourseDefaultCategory.NameAr.Contains(searchString)||
                                 p.Course.CourseDefaultCategory.NameEn.Contains(searchString)) &&
                                 p.CourseId == CourseId && !p.Deleted;
            }
            else
            {
                Criteria = p => p.CourseId == CourseId && !p.Deleted;
            }
        }
    }
}
