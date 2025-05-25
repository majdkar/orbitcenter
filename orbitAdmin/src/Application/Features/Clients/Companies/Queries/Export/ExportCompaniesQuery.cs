using System;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Entities.Clients;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.Clients;
using SchoolV01.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;



namespace SchoolV01.Application.Features.Clients.Companies.Queries.Export
{
    public class ExportCompaniesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportCompaniesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportCompaniesQueryHandler : IRequestHandler<ExportCompaniesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCompaniesQueryHandler> _localizer;

        public ExportCompaniesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCompaniesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companyFilterSpec = new CompanyFilterSpecification();
            var companies = await _unitOfWork.Repository<Company>().Entities

                .Specify(companyFilterSpec)

                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(companies, mappers: new Dictionary<string, Func<Company, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["EnglishName"], item => item.NameEn },
                { _localizer["ArabicName"],item => item.NameAr },
                { _localizer["ResponsiblePersonNameAr"],item => item.ResponsiblePersonNameAr },
                { _localizer["ResponsiblePersonNameEn"],item => item.ResponsiblePersonNameEn },
                { _localizer["ResponsiblePersonMobile"],item => item.ResponsiblePersonMobile },
                { _localizer["CountryNameEn"],item => item.Country.NameAr },
                { _localizer["CountryNameAr"],item => item.Country.NameEn },
                { _localizer["Phone"],item => item.Phone },
                { _localizer["Email"],item => item.Email },
                { _localizer["Address"],item => item.Address },
                { _localizer["Website"],item => item.Website },
                { _localizer["LicenseIssuingDate"],item => item.LicenseIssuingDate },
                { _localizer["AdditionalInfo"],item => item.AdditionalInfo },

            }, sheetName: _localizer["Companies"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
