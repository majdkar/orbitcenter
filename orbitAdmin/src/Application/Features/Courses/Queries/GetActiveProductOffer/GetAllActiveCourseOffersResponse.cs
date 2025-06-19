using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetActiveCourseOffer
{
    public class GetAllActiveCourseOffersResponse
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public string? CourseNameAr { get; set; }
        public string? CourseNameEn { get; set; }
        public string? CourseImageUrl { get; set; }



        public decimal? DiscountRatio { get; set; }

        public decimal? Price { get; set; }
        public decimal? NewPrice { get; set; }
        public decimal? OldPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
