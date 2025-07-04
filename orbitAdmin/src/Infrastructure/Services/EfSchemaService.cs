using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Identity;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Models;
using SchoolV01.Infrastructure.Contexts;
using SchoolV01.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Infrastructure.Services
{

   

   public class EfSchemaService
    {
        private readonly BlazorHeroContext _context;


        public EfSchemaService(BlazorHeroContext context)
        {
            _context = context;
        }

         public List<TableSchema> GetSchema()
        {
            var schema = new List<TableSchema>();
            var model = _context.Model;

            foreach (var entityType in model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                var table = new TableSchema { TableName = tableName };

                foreach (var property in entityType.GetProperties())
                {
                    var column = new ColumnSchema
                    {
                        ColumnName = property.GetColumnName(),
                        DataType = property.ClrType.Name,
                        IsNullable = property.IsNullable
                    };

                    table.Columns.Add(column);
                }

                schema.Add(table);
            }

            return schema;
        }



        public async Task<object> GetAllProductsAsync(string q)
        {
            // Using a raw SQL query to get all users
            //string query = q;
            //return await _context.Products.FromSqlRaw(query).ToListAsync();

            //var products = await _context.Products
            //    .FromSqlRaw(q)
            //    .ToListAsync();

            //var viewProducts = products.Select(p => new ViewProduct
            //{
            //    NameAr = p.NameAr,
            //    NameEn = p.NameEn,
            //    Price = p.Price
            //}).ToList();

            //return viewProducts;


            //using (var command = _context.Database.GetDbConnection().CreateCommand())
            //{
            //    command.CommandText = q;
            //    await _context.Database.OpenConnectionAsync();

            //    var result = await command.ExecuteScalarAsync();

            //    return JsonSerializer.Serialize(result);
            //}


            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = q;
                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    var results = new List<Dictionary<string, object>>();

                    while (await reader.ReadAsync())
                    {
                        var row = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }

                        results.Add(row);
                    }

                    return JsonSerializer.Serialize(results, new JsonSerializerOptions
                    {
                        WriteIndented = true // Optional: for pretty-printing
                    });
                }
            }
        }
    }


    public class ViewProduct
    {

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string NameGe { get; set; }


        public string DescriptionEn1 { get; set; }
        public string DescriptionEn2 { get; set; }
        public string DescriptionEn3 { get; set; }
        public string DescriptionEn4 { get; set; }

        public string DescriptionAr1 { get; set; }
        public string DescriptionAr2 { get; set; }
        public string DescriptionAr3 { get; set; }
        public string DescriptionAr4 { get; set; }
        public string DescriptionGe1 { get; set; }
        public string DescriptionGe2 { get; set; }
        public string DescriptionGe3 { get; set; }
        public string DescriptionGe4 { get; set; }

        public string Code { get; set; }



        public int? ProductParentCategoryId { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductSubSubCategoryId { get; set; }
        public int? ProductSubSubSubCategoryId { get; set; }

        public int? ProductDefaultCategoryId { get; set; }



        public decimal? Price { get; set; }
        public int Order { get; set; } = 0;
        public bool IsVisible { get; set; } = true;
        public bool IsRecent { get; set; } = false;

        public string Plan { get; set; }

        public string ProductImageUrl1 { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }



        public string Keywords { get; set; }
        public string SeoDescription { get; set; }
    }
}
