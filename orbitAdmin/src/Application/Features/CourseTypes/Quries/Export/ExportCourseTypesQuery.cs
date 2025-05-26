using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.CourseTypes.Queries
{
    public class ExportCourseTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCourseTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCourseTypesQueryHandler : IRequestHandler<ExportCourseTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCourseTypesQueryHandler> _localizer;

        public ExportCourseTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCourseTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCourseTypesQuery request, CancellationToken cancellationToken)
        {
            var CourseTypes = await _unitOfWork.Repository<Country>().Entities
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(CourseTypes, mappers: new Dictionary<string, Func<Country, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["ArabicName"], item => item.NameAr },
               

                { _localizer["EnglishName"],item => item.NameEn },
               
            }, sheetName: _localizer["CourseTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }

}
