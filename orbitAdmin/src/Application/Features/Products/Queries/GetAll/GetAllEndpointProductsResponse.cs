using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllEndpointProductsResponse
    {
        public int Id { get; set; }
        public string EndpointAr { get; set; }
        public string EndpointEn { get; set; }
        public string EndpointGe { get; set; }
    }
}
