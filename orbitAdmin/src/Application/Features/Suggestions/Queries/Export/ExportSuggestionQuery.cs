using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


using SchoolV01.Domain.Entities.Suggestions;
using SchoolV01.Application.Specifications.Suggestions;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Features.Suggestions.Queries.Export
{
    public class ExportSuggestionQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }
        public SuggestionType Type { get; set; }

        public ExportSuggestionQuery(string searchString = "", SuggestionType type = 0)
        {
            SearchString = searchString;
            Type = type;
        }
    }
    internal class ExportSuggestionQueryHandler : IRequestHandler<ExportSuggestionQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportSuggestionQueryHandler> _localizer;

        public ExportSuggestionQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportSuggestionQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportSuggestionQuery request, CancellationToken cancellationToken)
        {
            var orderFilterSpec = new SuggestionFilterSpecification(request.SearchString,request.Type);
            var Notifications = await _unitOfWork.Repository<Suggestion>().Entities
                .Specify(orderFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Notifications, mappers: new Dictionary<string, Func<Suggestion, object>>
            {
                { _localizer["Id"], item => item.Id },
                 { _localizer["UserName"], item => item.UserName},

                 { _localizer["Email"], item => item.Email},
                { _localizer["Description"], item => item.Description },
                            }, sheetName: _localizer["Suggestions"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
