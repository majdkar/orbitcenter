using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Specifications.Catalog;

namespace SchoolV01.Application.Features.Products.Queries.Export
{
    public class ExportProductOrdersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        //public int CompanyId { get; set; }

        public ExportProductOrdersQuery(string searchString = "")
        {
            SearchString = searchString;
            //CompanyId = companyId;
        }
    }

    internal class ExportProductOrdersQueryHandler : IRequestHandler<ExportProductOrdersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportProductOrdersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductOrdersQuery request, CancellationToken cancellationToken)
        {
            var isArabic = CultureInfo.CurrentCulture.Name.Contains("ar");

            var ProductFilterSpec = new ProductOrderFilterSpecification();
            var Products = await _unitOfWork.Repository<ProductOrder>().Entities
                .Specify(ProductFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Products, mappers: new Dictionary<string, Func<ProductOrder, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["OrderDate"], item => item.OrderDate },
                { _localizer["OrderNumber"], item => item.OrderNumber },

                { _localizer["Price"], item => item.TotalPrice }
            }, sheetName: _localizer["Products"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}