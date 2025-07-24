using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Specifications.Courses
{
    public class CourseByEndpointFilterSpecification : HeroSpecification<Course>
    {
        public CourseByEndpointFilterSpecification(string Endpoint)
        {
            Includes.Add(x => x.CourseDefaultCategory);
      

            Criteria = x => x.EndpointAr == Endpoint || x.EndpointEn == Endpoint || x.EndpointGe == Endpoint;
        }
    }
}
