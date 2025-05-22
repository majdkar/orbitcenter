using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.GeneralSettings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Products.Queries.Export;
using SchoolV01.Core.Entities;

namespace SchoolV01.Application.Features.ProductCategories.Queries.Export
{
    public class ExportProductCategoriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProductCategoriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProductCategoriesQueryHandler : IRequestHandler<ExportProductCategoriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductCategoriesQueryHandler> _localizer;

        public ExportProductCategoriesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductCategoriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var productCategoryFilterSpec = new ProductCategoryFilterSpecification(request.SearchString);
            var productCategories= await _unitOfWork.Repository<ProductCategory>().Entities
                .Specify(productCategoryFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(productCategories, mappers: new Dictionary<string, Func<ProductCategory, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameEn"], item => item.NameEn },
                 { _localizer["NameAr"], item => item.NameAr },
                { _localizer["DescriptionAr"], item => item.DescriptionAr1 },
                { _localizer["DescriptionEn"], item => item.DescriptionEn1 },
                 { _localizer["Order"], item => item.Order },
            }, sheetName: _localizer["ProductCategories"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
