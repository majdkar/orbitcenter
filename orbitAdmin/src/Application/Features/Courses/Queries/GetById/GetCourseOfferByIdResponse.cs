using System;


namespace SchoolV01.Application.Features.Courses.Queries.GetById
{
    public class GetCourseOfferByIdResponse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
