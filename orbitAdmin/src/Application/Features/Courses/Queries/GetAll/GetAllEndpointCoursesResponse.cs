using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllEndpointCoursesResponse
    {
        public int Id { get; set; }
     




        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }


    }
}
