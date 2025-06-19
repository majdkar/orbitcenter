using Newtonsoft.Json;
using System;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAllPaged
{
    public class GetAllPagedActiveCourseOffersResponse
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        [JsonProperty("CourseNameAr")]
        public string NameAr { get; set; }
        [JsonProperty("CourseNameEn")]
        public string NameEn { get; set; }
        [JsonProperty("CourseDescriptionAr")]
        public string? DescriptionAr4 { get; set; }
        public string? DescriptionAr1 { get; set; }
        public string? DescriptionAr2 { get; set; }
        public string? DescriptionAr3 { get; set; }
        [JsonProperty("CourseDescriptionEn")]
        public string? DescriptionEn1 { get; set; }
        public string? DescriptionEn2 { get; set; }
        public string? DescriptionEn3 { get; set; }
        public string? DescriptionEn4 { get; set; }    
        
        
        [JsonProperty("CourseDescriptionGe")]
        public string? DescriptionGe1 { get; set; }
        public string? DescriptionGe2 { get; set; }
        public string? DescriptionGe3 { get; set; }
        public string? DescriptionGe4 { get; set; }


        [JsonProperty("CourseCode")]
        public string? Code { get; set; }
 
        [JsonProperty("Plan")]
        public string? Plan { get; set; }
 

        [JsonProperty("CourseCategoryId")]
        public int CategoryId { get; set; }
        [JsonProperty("CourseCategory")]
        public virtual CourseCategory Category { get; set; }
        [JsonProperty("CourseParentCategoryId")]
        public int? ParentCategoryId { get; set; }
        [JsonProperty("CourseParentCategory")]
        public virtual CourseCategory ParentCategory { get; set; }
     
      
    

        [JsonProperty("CoursePrice")]
        public decimal? Price { get; set; }
        [JsonProperty("CourseOrder")]
        public int Order { get; set; } = 0;
        [JsonProperty("CourseIsVisible")]
        public bool IsVisible { get; set; } = true;
        [JsonProperty("CourseIsRecent")]
        public bool IsRecent { get; set; } = false;

      
        [JsonProperty("CourseCourseImageUrl")]
        public string CourseImageUrl1 { get; set; }
        public string CourseImageUrl2 { get; set; }
        public string CourseImageUrl3 { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }



    }
}
