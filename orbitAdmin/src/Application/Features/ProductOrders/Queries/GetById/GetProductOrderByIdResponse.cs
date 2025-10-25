using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Enums;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Application.Features.ProductOrders.Queries.GetById
{
    public class GetProductOrderByIdResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string ClientNameEn { get; set; }
        public string ClientNameAr { get; set; }
        public string ClientType { get; set; }
        public string Status { get; set; }

        public DateTime? OrderDate { get; set; } = DateTime.UtcNow;

        public string Notes { get; set; }

        public string PaymentStatus { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalPrice { get; set; }

        public int? PayTypeId { get; set; }
        public PayType PayType { get; set; }

        public string PaymentTransactionNumber { get; set; }

        // كل العناصر (المنتجات) داخل الطلب
        public ICollection<ProductOrderItem> Items { get; set; } = new List<ProductOrderItem>();
    }
}
