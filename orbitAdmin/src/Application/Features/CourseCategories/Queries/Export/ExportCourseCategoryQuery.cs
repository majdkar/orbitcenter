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
using SchoolV01.Core.Entities;

namespace SchoolV01.Application.Features.CourseCategories.Queries.Export
{
    public class ExportCourseCategoriesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCourseCategoriesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCourseCategoriesQueryHandler : IRequestHandler<ExportCourseCategoriesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCourseCategoriesQueryHandler> _localizer;

        public ExportCourseCategoriesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCourseCategoriesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCourseCategoriesQuery request, CancellationToken cancellationToken)
        {
            var CourseCategoryFilterSpec = new CourseCategoryFilterSpecification(request.SearchString);
            var CourseCategories= await _unitOfWork.Repository<CourseCategory>().Entities
                .Specify(CourseCategoryFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(CourseCategories, mappers: new Dictionary<string, Func<CourseCategory, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameEn"], item => item.NameEn },
                 { _localizer["NameAr"], item => item.NameAr },
                { _localizer["DescriptionAr"], item => item.DescriptionAr1 },
                { _localizer["DescriptionEn"], item => item.DescriptionEn1 },
                 { _localizer["Order"], item => item.Order },
            }, sheetName: _localizer["CourseCategories"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
