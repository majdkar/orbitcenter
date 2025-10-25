using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged
{
    public class GetAllPagedSearchCourseOrdersQuery : IRequest<PaginatedResult<GetAllPagedCourseOrdersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }

        public string OrderNumber { get; set; }
        public int ClientId { get; set; }
        public int CourseId { get; set; }

        public decimal FromPrice { get; set; }

        public decimal ToPrice { get; set; }


        public GetAllPagedSearchCourseOrdersQuery(int pageNumber, int pageSize, string searchString, string orderBy, string orderNumber, int clientId, int courseId, decimal fromprice, decimal toprice)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
             OrderNumber = orderNumber;
            ClientId = clientId;
            CourseId = courseId;
            FromPrice = fromprice;
            ToPrice = toprice;
        }
    }

    internal class GetAllPagedSearchCourseOrdersQueryHandler : IRequestHandler<GetAllPagedSearchCourseOrdersQuery, PaginatedResult<GetAllPagedCourseOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchCourseOrdersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> Handle(GetAllPagedSearchCourseOrdersQuery request, CancellationToken cancellationToken)
        {


            Expression<Func<CourseOrder, GetAllPagedCourseOrdersResponse>> expression = e => new GetAllPagedCourseOrdersResponse
            {
                Id = e.Id,
                Price = e.Price,
                Notes = e.Notes,
                Course = e.Course,
                CourseId = e.CourseId,
                ClientId = e.ClientId,
                Status = e.Status,
                PaymentStatus = e.PaymentStatus,
                OrderNumber = e.OrderNumber,
                Client = e.Client,
                ClientType = e.ClientType,

                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,

                PaymentTransactionNumber = e.PaymentTransactionNumber,
                PayTypeId = e.PayTypeId,
                PayType = e.PayType,
            };
            var CourseOrderFilterSpec = new CourseOrderSearchFilterSpecification(request.SearchString, request.OrderNumber, request.CourseId, request.ClientId, request.FromPrice, request.ToPrice);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<CourseOrder>().Entities
                   .Specify(CourseOrderFilterSpec)
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<CourseOrder>().Entities
                   .Specify(CourseOrderFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}