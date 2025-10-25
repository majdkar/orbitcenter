using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.GeneralSettings;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.PayTypes.Queries.Export
{
    public class ExportPayTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPayTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPayTypesQueryHandler : IRequestHandler<ExportPayTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPayTypesQueryHandler> _localizer;

        public ExportPayTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportPayTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPayTypesQuery request, CancellationToken cancellationToken)
        {
            var PayTypeFilterSpec = new PayTypeFilterSpecification(request.SearchString);
            var PayTypes = await _unitOfWork.Repository<PayType>().Entities
                .Specify(PayTypeFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(PayTypes, mappers: new Dictionary<string, Func<PayType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameAr"], item => item.NameAr },
                { _localizer["NameEn"], item => item.NameEn },

            }, sheetName: _localizer["PayTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
