﻿using Newtonsoft.Json;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductSeosResponse
    {
        public int Id { get; set; }

        [ForeignKey("ProductCategory")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }



        public string MetaTitleAr { get; set; }
        public string MetaTitleEn { get; set; }
        public string MetaTitleGe { get; set; }


        public string MetaNameAr { get; set; }
        public string MetaNameEn { get; set; }
        public string MetaNameGe { get; set; }

        public string MetaUrlAr { get; set; }
        public string MetaUrlEn { get; set; }
        public string MetaUrlGe { get; set; }


        public string MetaKeywordsAr { get; set; }
        public string MetaKeywordsEn { get; set; }
        public string MetaKeywordsGe { get; set; }

        public string MetaDescriptionsAr { get; set; }
        public string MetaDescriptionsEn { get; set; }
        public string MetaDescriptionsGe { get; set; }


        public string ImageAlt1Ar { get; set; }
        public string ImageAlt1En { get; set; }
        public string ImageAlt1Ge { get; set; }

        public string ImageAlt2Ar { get; set; }
        public string ImageAlt2En { get; set; }
        public string ImageAlt2Ge { get; set; }

        public string ImageAlt3Ar { get; set; }
        public string ImageAlt3En { get; set; }
        public string ImageAlt3Ge { get; set; }

        public string ImageAlt4Ar { get; set; }
        public string ImageAlt4En { get; set; }
        public string ImageAlt4Ge { get; set; }


        public string MetaRobots { get; set; }


    }
}
