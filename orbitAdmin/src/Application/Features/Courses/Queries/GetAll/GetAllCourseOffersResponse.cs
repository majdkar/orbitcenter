using System;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCourseOffersResponse
    {
        public int Id { get; set; }
        public virtual Course Course { get; set; }
        public int CourseId { get; set; }
   

        public decimal? NewPrice { get; set; }
        public decimal? DiscountRatio { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
