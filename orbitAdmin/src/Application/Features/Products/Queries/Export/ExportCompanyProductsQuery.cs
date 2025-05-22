using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Queries.Export
{
    public class ExportCompanyProductsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        //public int CompanyId { get; set; }

        public ExportCompanyProductsQuery(string searchString = "")
        {
            SearchString = searchString;
            //CompanyId = companyId;
        }
    }

    internal class ExportProductsQueryHandler : IRequestHandler<ExportCompanyProductsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;

        public ExportProductsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCompanyProductsQuery request, CancellationToken cancellationToken)
        {
            var productFilterSpec = new ProductFilterSpecification();
            var products = await _unitOfWork.Repository<Product>().Entities
                .Specify(productFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Product, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameAr"], item => item.NameAr },
                { _localizer["NameEn"], item => item.NameEn },
                { _localizer["Code"], item => item.Code },

                { _localizer["Price"], item => item.Price }
            }, sheetName: _localizer["Products"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}