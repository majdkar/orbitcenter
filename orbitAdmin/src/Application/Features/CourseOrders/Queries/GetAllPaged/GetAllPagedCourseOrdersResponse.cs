using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged
{
    public class GetAllPagedCourseOrdersResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientNameEn { get; set; }
        public string ClientNameAr { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public string Status { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderNumber { get; set; }

        public decimal Price { get; set; }

    }
}