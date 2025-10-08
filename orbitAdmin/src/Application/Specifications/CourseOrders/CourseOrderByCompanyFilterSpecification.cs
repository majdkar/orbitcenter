using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class CourseOrderByCompanyFilterSpecification : HeroSpecification<CourseOrder>
    {
        public CourseOrderByCompanyFilterSpecification(string searchString)
        {
            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.Deleted &&
                                (p.Course.NameAr.Contains(searchString) ||
                                p.Course.NameEn.Contains(searchString) ||
                                p.OrderNumber.Contains(searchString)
                               );
            }
            else
            {
                Criteria = p =>  !p.Deleted;
            }
        }
    }
}