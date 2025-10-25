using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Enums;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetById
{
    public class GetCourseOrderByIdResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientNameEn { get; set; }
        public string ClientNameAr { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string ClientType { get; set; }
        public string Status { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderNumber { get; set; }

        public decimal Price { get; set; }

        public int? PayTypeId { get; set; }
        public PayType PayType { get; set; }

        public string PaymentTransactionNumber { get; set; }
    }
}
