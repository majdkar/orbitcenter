using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseByNameFilterSpecification : HeroSpecification<Course>
    {
        public CourseByNameFilterSpecification(string Name)
        {
            Includes.Add(x => x.CourseDefaultCategory);
      

            Criteria = x => x.NameEn == Name || x.NameAr == Name || x.NameGe == Name;
        }
    }
}
