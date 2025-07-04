﻿using MediatR;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Linq.Dynamic.Core;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAllPaged
{
    public class GetAllPagedCourseOffersQuery : IRequest<PaginatedResult<GetAllPagedCourseOffersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public GetAllPagedCourseOffersQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllCourseOffersQueryHandler : IRequestHandler<GetAllPagedCourseOffersQuery, PaginatedResult<GetAllPagedCourseOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCourseOffersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCourseOffersResponse>> Handle(GetAllPagedCourseOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CourseOffer, GetAllPagedCourseOffersResponse>> expression = e => new GetAllPagedCourseOffersResponse
            {
                Id = e.Id,
                Course = e.Course,
                Category = e.Course.CourseDefaultCategory,
                CourseId = e.CourseId,
                CourseImageUrl1 = e.Course.CourseImageUrl1,
                CourseImageUrl2 = e.Course.CourseImageUrl1,
                CourseImageUrl3 = e.Course.CourseImageUrl1,
                NameAr = e.Course.NameAr,
                NameEn = e.Course.NameEn,
                Code = e.Course.Code,

                DescriptionAr1 = e.Course.DescriptionAr1,
                DescriptionAr2 = e.Course.DescriptionAr2,
                DescriptionAr3 = e.Course.DescriptionAr3,
                DescriptionAr4 = e.Course.DescriptionAr4,
                DescriptionEn1 = e.Course.DescriptionEn1,
                DescriptionEn2 = e.Course.DescriptionEn1,
                DescriptionEn3 = e.Course.DescriptionEn1,
                DescriptionEn4 = e.Course.DescriptionEn1,

                DescriptionGe1 = e.Course.DescriptionGe1,
                DescriptionGe2 = e.Course.DescriptionGe2,
                DescriptionGe3 = e.Course.DescriptionGe3,
                DescriptionGe4 = e.Course.DescriptionGe4,


                Order = e.Course.Order,
                IsVisible = e.Course.IsVisible,
                IsRecent = e.Course.IsRecent,
                DiscountRatio = e.DiscountRatio,
                EndDate = e.EndDate,
                NewPrice = e.NewPrice,
                StartDate = e.StartDate,
                Plan = e.Course.Plan,
                Price = e.Course.Price


            };
            var CourseOfferFilterSpec = new CourseOfferPagedSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<CourseOffer>().Entities
                   .Specify(CourseOfferFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<CourseOffer>().Entities
                   .Specify(CourseOfferFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
