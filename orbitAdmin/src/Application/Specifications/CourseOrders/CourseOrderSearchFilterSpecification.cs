using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Specifications.Catalog
{
    public class CourseOrderSearchFilterSpecification : HeroSpecification<CourseOrder>
    {
        public CourseOrderSearchFilterSpecification(string searchString, string OrderNumber, int CourseId, int ClientId,  decimal fromprice, decimal toprice)
        {
            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");

            //Criteria = p => !p.IsDeleted;
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.Deleted &&
                                 
                                 (OrderNumber == null ? p.OrderNumber.Length > 0 : p.OrderNumber.Contains(OrderNumber) &&
                                (CourseId == 0 ? true : p.CourseId == CourseId) &&
                                (ClientId == 0 ? true : p.ClientId == ClientId) &&
                                (fromprice == 0 ? p.Price >= 0 : p.Price >= fromprice) &&
                                (toprice == 0 ? p.Price >= 0 : p.Price <= toprice));
      
            }
            else
            {
                Criteria = p => !p.Deleted &&
                        (string.IsNullOrEmpty(searchString) || p.OrderNumber.Contains(searchString)) &&
                        (string.IsNullOrEmpty(OrderNumber) || p.OrderNumber.Contains(OrderNumber)) &&
                        (CourseId == 0 || p.CourseId == CourseId) &&
                        (ClientId == 0 || p.ClientId == ClientId) &&
                        (fromprice == 0 || p.Price >= fromprice) &&
                        (toprice == 0 || p.Price <= toprice);

            }
        }
    }
}