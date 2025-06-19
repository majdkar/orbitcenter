using SchoolV01.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolV01.Domain.Entities.Courses
{
    public class CourseOffer : AuditableEntity<int>
    {
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
