using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Specifications.Courses
{
    public class AllPagedActiveCourseOffersFilterSpecification : HeroSpecification<CourseOffer>
    {
        public AllPagedActiveCourseOffersFilterSpecification(string searchString)
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
