﻿using MediatR;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Application.Specifications.Courses;

using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Courses.Queries.GetAllPaged
{
    public class GetAllPagedCoursesQuery : IRequest<PaginatedResult<GetAllPagedCoursesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedCoursesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
           
        }
    }

    internal class GetAllCoursesQueryHandler : IRequestHandler<GetAllPagedCoursesQuery, PaginatedResult<GetAllPagedCoursesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCoursesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> Handle(GetAllPagedCoursesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Course, GetAllPagedCoursesResponse>> expression = e => new GetAllPagedCoursesResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4,


                DescriptionEn1 = e.DescriptionEn1,
                DescriptionEn2 = e.DescriptionEn2,
                DescriptionEn3 = e.DescriptionEn3,
                DescriptionEn4 = e.DescriptionEn4,

                DescriptionGe1 = e.DescriptionGe1,
                DescriptionGe2 = e.DescriptionGe2,
                DescriptionGe3 = e.DescriptionGe3,
                DescriptionGe4 = e.DescriptionGe4,


                CourseParentCategoryId = e.CourseParentCategoryId,
                CourseSubCategoryId = e.CourseSubCategoryId,
                CourseSubSubCategoryId = e.CourseSubSubSubCategoryId,
                CourseSubSubSubCategoryId = e.CourseSubSubSubCategoryId,
                CourseDefaultCategoryId = e.CourseDefaultCategoryId.Value,


                Price = e.Price,
                Code = e.Code,
                EndpointAr = e.EndpointAr,
                EndpointGe = e.EndpointGe,
                EndpointEn = e.EndpointEn,
                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,

                NameGe = e.NameGe,
                Plan = e.Plan,
                CourseDefaultCategory = e.CourseDefaultCategory,
                CourseImageUrl1 = e.CourseImageUrl1,
                CourseImageUrl2 = e.CourseImageUrl2,
                CourseImageUrl3 = e.CourseImageUrl3,
                CourseOffers = e.CourseOffers,
                 SeoDescription = e.SeoDescription,
                  Keywords = e.Keywords,
                CourseTypeId = e.CourseTypeId,
                NumSesstions = e.NumSesstions,
                StartDate = e.StartDate,
                TeacherNameEn = e.TeacherNameEn,
                TeacherNameAr = e.TeacherNameAr,
                TeacherNameGe = e.TeacherNameGe,
                StartEnd = e.StartEnd,
                NumMaxStudent = e.NumMaxStudent,
                CourseType = e.CourseType,

            };
            var CourseFilterSpec = new CourseByCompanyFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Course>().Entities
                   //.Specify(CourseFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Course>().Entities
                   .Specify(CourseFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}