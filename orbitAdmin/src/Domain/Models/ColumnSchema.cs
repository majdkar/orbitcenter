﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Domain.Models
{
   public  class ColumnSchema
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public bool IsNullable { get; set; }
    }
}
