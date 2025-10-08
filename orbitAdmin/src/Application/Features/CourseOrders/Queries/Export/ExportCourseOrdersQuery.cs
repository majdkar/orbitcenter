using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Application.Specifications.Courses;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Queries.Export
{
    public class ExportCourseOrdersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        //public int CompanyId { get; set; }

        public ExportCourseOrdersQuery(string searchString = "")
        {
            SearchString = searchString;
            //CompanyId = companyId;
        }
    }

    internal class ExportCourseOrdersQueryHandler : IRequestHandler<ExportCourseOrdersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportCoursesQueryHandler> _localizer;

        public ExportCourseOrdersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportCoursesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportCourseOrdersQuery request, CancellationToken cancellationToken)
        {
            var isArabic = CultureInfo.CurrentCulture.Name.Contains("ar");

            var CourseFilterSpec = new CourseOrderFilterSpecification();
            var Courses = await _unitOfWork.Repository<CourseOrder>().Entities
                .Specify(CourseFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(Courses, mappers: new Dictionary<string, Func<CourseOrder, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Course Name"], item => isArabic ? item.Course.NameAr : item.Course.NameEn},
                { _localizer["OrderDate"], item => item.OrderDate },
                { _localizer["OrderNumber"], item => item.OrderNumber },

                { _localizer["Price"], item => item.Price }
            }, sheetName: _localizer["Courses"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}