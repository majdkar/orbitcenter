using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.Clients;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Clients.Persons.Queries.Export
{
    public class ExportPersonsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPersonsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }
    internal class ExportPersonsQueryHandler : IRequestHandler<ExportPersonsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPersonsQueryHandler> _localizer;

        public ExportPersonsQueryHandler(IExcelService excelService, IUnitOfWork<int> unitOfWork, IStringLocalizer<ExportPersonsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPersonsQuery request, CancellationToken cancellationToken)
        {
            var personFilterSpec = new PersonFilterSpecification(request.SearchString);
            var persons = await _unitOfWork.Repository<Person>().Entities
                .Specify(personFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(persons, mappers: new Dictionary<string, Func<Person, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Full Name"], item => item.FullName },
                { _localizer["BirthDate"],item => item.BirthDate },
                { _localizer["Sex"], item => item.Sex },
                { _localizer["Countryality"],item => item.Country.NameAr },
                { _localizer["Email"], item => item.Email },
                { _localizer["Fax"],item => item.Fax },
                { _localizer["MailBox"], item => item.MailBox },
                { _localizer["Phone"],item => item.Phone },

            }, sheetName: _localizer["Persons"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
