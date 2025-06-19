using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.Courses;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Queries.Export
{
    public class ExportCompanyCoursesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        //public int CompanyId { get; set; }

        public ExportCompanyCoursesQuery(string searchString = "")
        {
            SearchString = searchString;
            //CompanyId = companyId;
        }
    }

    internal class ExportCoursesQueryHandler : IRequestHandler<ExportCompanyCoursesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCoursesQueryHandler> _localizer;

        public ExportCoursesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCoursesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCompanyCoursesQuery request, CancellationToken cancellationToken)
        {
            var CourseFilterSpec = new CourseFilterSpecification();
            var Courses = await _unitOfWork.Repository<Course>().Entities
                .Specify(CourseFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Courses, mappers: new Dictionary<string, Func<Course, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["NameAr"], item => item.NameAr },
                { _localizer["NameEn"], item => item.NameEn },
                { _localizer["Code"], item => item.Code },

                { _localizer["Price"], item => item.Price }
            }, sheetName: _localizer["Courses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}