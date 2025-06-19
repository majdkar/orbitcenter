using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;
using static SchoolV01.Shared.Constants.Permission.Permissions;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseFilterSpecification : HeroSpecification<Course>
    {
        public CourseFilterSpecification()
        {
     
            Criteria = p =>  !p.Deleted;
        }
    }
}
