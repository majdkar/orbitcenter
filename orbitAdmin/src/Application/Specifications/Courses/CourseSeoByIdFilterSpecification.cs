using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseSeoByIdFilterSpecification : HeroSpecification<CourseSeo>
    {
        public CourseSeoByIdFilterSpecification(int id)
        {
            Includes.Add(x => x.Course);
      

            Criteria = x => x.Id == id;
        }
    }
}
