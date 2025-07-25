﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCoursesResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }

        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }
        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public string Code { get; set; }

        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }

        [ForeignKey("CourseParentCategory")]
        public int? CourseParentCategoryId { get; set; }
        public virtual CourseCategory CourseParentCategory { get; set; }
        public int? CourseSubCategoryId { get; set; }
        public int? CourseSubSubCategoryId { get; set; }
        public int? CourseSubSubSubCategoryId { get; set; }

        [ForeignKey("CourseDefaultCategory")]
        public int? CourseDefaultCategoryId { get; set; }
        public virtual CourseCategory CourseDefaultCategory { get; set; }



        public decimal? Price { get; set; }
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool IsRecent { get; set; } = false;

        public string Plan { get; set; }

        public string CourseImageUrl1 { get; set; }
        public string CourseImageUrl2 { get; set; }
        public string CourseImageUrl3 { get; set; }

        public virtual List<CourseOffer> CourseOffers { get; set; }
        public string Keywords { get; set; }
        public string SeoDescription { get; set; }

        public string TeacherNameAr { get; set; }
        public string TeacherNameEn { get; set; }
        public string TeacherNameGe { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? StartEnd { get; set; }

        public int NumSesstions { get; set; }
        public int NumMaxStudent { get; set; }

        public int? CourseTypeId { get; set; }
        public virtual CourseType CourseType { get; set; }

    }
}
