using System;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCourseSeosResponse
    {
        public int Id { get; set; }

        [ForeignKey("CourseCategory")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }



        public string MetaTitleAr { get; set; }
        public string MetaTitleEn { get; set; }
        public string MetaTitleGe { get; set; }


        public string MetaNameAr { get; set; }
        public string MetaNameEn { get; set; }
        public string MetaNameGe { get; set; }

        public string MetaUrlAr { get; set; }
        public string MetaUrlEn { get; set; }
        public string MetaUrlGe { get; set; }


        public string MetaKeywordsAr { get; set; }
        public string MetaKeywordsEn { get; set; }
        public string MetaKeywordsGe { get; set; }

        public string MetaDescriptionsAr { get; set; }
        public string MetaDescriptionsEn { get; set; }
        public string MetaDescriptionsGe { get; set; }


        public string ImageAlt1Ar { get; set; }
        public string ImageAlt1En { get; set; }
        public string ImageAlt1Ge { get; set; }

        public string ImageAlt2Ar { get; set; }
        public string ImageAlt2En { get; set; }
        public string ImageAlt2Ge { get; set; }

        public string ImageAlt3Ar { get; set; }
        public string ImageAlt3En { get; set; }
        public string ImageAlt3Ge { get; set; }

        public string ImageAlt4Ar { get; set; }
        public string ImageAlt4En { get; set; }
        public string ImageAlt4Ge { get; set; }


        public string MetaRobots { get; set; }

    }
}
