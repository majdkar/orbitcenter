using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Linq.Expressions;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllPagedCourseOffersQuery : IRequest<PaginatedResult<GetAllCourseOffersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public int CourseId { get; set; }
        public GetAllPagedCourseOffersQuery(int CourseId,int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            CourseId = CourseId;
        }
    }

    internal class GetAllPagedCourseOffersQueryHandler : IRequestHandler<GetAllPagedCourseOffersQuery, PaginatedResult<GetAllCourseOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedCourseOffersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllCourseOffersResponse>> Handle(GetAllPagedCourseOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CourseOffer, GetAllCourseOffersResponse>> expression = e => new GetAllCourseOffersResponse
            {
                Id = e.Id,
                //Course = e.Course,
                CourseId = e.CourseId,
                DiscountRatio = e.DiscountRatio,
                EndDate = e.EndDate,
                NewPrice = e.NewPrice,
                StartDate = e.StartDate,
                  
                //ParentCategory=e.Course.CourseCategory.ParentCategory,
                //ParentCategoryId=e.Course.CourseCategory.ParentCategoryId



            };
            var CourseOfferFilterSpec = new CourseOfferSpecification(request.CourseId, request.SearchString);
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