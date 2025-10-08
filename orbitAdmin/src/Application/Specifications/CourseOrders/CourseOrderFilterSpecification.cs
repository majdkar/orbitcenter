using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;
using static SchoolV01.Shared.Constants.Permission.Permissions;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseOrderFilterSpecification : HeroSpecification<CourseOrder>
    {
        public CourseOrderFilterSpecification()
        {
            Includes.Add(x => x.Client);
            IncludeStrings.Add("Client.Person");
            IncludeStrings.Add("Client.Company");
     
            Criteria = p =>  !p.Deleted;
        }
    }
}
