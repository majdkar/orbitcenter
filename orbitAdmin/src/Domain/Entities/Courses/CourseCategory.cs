using SchoolV01.Domain.Contracts;

namespace SchoolV01.Core.Entities
{
    public class CourseCategory : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }
        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }


        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public int? ParentCategoryId { get; set; }
        public virtual CourseCategory ParentCategory { get; set; }

        public int Order { get; set; } = 0;
        public string ImageDataURL1 { get; set; }
        public string ImageDataURL2 { get; set; }
        public string ImageDataURL3 { get; set; }


    }
}
