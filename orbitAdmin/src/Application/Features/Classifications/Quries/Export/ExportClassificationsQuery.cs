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

namespace SchoolV01.Application.Features.Classifications.Queries
{
    public class ExportClassificationsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportClassificationsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportClassificationsQueryHandler : IRequestHandler<ExportClassificationsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportClassificationsQueryHandler> _localizer;

        public ExportClassificationsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportClassificationsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportClassificationsQuery request, CancellationToken cancellationToken)
        {
            var Classifications = await _unitOfWork.Repository<Country>().Entities
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Classifications, mappers: new Dictionary<string, Func<Country, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["ArabicName"], item => item.NameAr },
               

                { _localizer["EnglishName"],item => item.NameEn },
               
            }, sheetName: _localizer["Classifications"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }

}
