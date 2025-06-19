using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using System;

namespace SchoolV01.Application.Specifications.Courses
{
    public class AllActiveCourseOfferFilterSpecification : HeroSpecification<CourseOffer>
    {
        public AllActiveCourseOfferFilterSpecification()
        {
            Includes.Add(p => p.Course);
            //Includes.Add(p => p.CourseWeight);
            //Includes.Add(p => p.CourseOptionSize);
            //IncludeStrings.Add("Course.Company");
            //IncludeStrings.Add("Course.CourseImages");
            Criteria = p => p.StartDate <= DateTime.Now.Date && p.EndDate >= DateTime.Now.Date && !p.Deleted;
        }
    }
}
