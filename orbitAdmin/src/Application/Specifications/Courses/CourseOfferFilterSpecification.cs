using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseOfferFilterSpecification : HeroSpecification<CourseOffer>
    {
        public CourseOfferFilterSpecification(int CourseId)
        {
            Criteria = p => p.CourseId == CourseId && !p.Deleted;
        }
    }
}