using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class CourseOrderByClientFilterSpecification : HeroSpecification<CourseOrder>
    {
        public CourseOrderByClientFilterSpecification(string searchString,int clientId)
        {
            Includes.Add(x => x.Client);
            Includes.Add(x => x.PayType);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.Deleted &&
                                   p.ClientId == clientId &&
                                (p.Course.NameAr.Contains(searchString) ||
                                p.Course.NameEn.Contains(searchString) ||
                                p.OrderNumber.Contains(searchString)
                               );
            }
            else
            {
                Criteria = p =>  !p.Deleted && p.ClientId == clientId;
            }
        }
    }
}