using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseOrderByIdFilterSpecification : HeroSpecification<CourseOrder>
    {
        public CourseOrderByIdFilterSpecification(int id)
        {
            Includes.Add(x => x.PayType);
            Includes.Add(x => x.Course);
            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");


            Criteria = x => x.Id == id;
        }
    }
}
