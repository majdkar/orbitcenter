using MediatR;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;

using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged
{
    public class GetAllPagedCourseOrdersByClientQuery : IRequest<PaginatedResult<GetAllPagedCourseOrdersResponse>>
    {
        public int ClientId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedCourseOrdersByClientQuery(int pageNumber, int pageSize, string searchString, string orderBy,int clientId)
        {
            PageNumber = pageNumber;
            ClientId = clientId;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
           
        }
    }

    internal class GetAllPagedCourseOrdersByClientQueryHandler : IRequestHandler<GetAllPagedCourseOrdersByClientQuery, PaginatedResult<GetAllPagedCourseOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedCourseOrdersByClientQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> Handle(GetAllPagedCourseOrdersByClientQuery request, CancellationToken cancellationToken)
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
                OrderDate = e.OrderDate,
                 ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                 ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
                ClientType = e.ClientType,
                PaymentTransactionNumber = e.PaymentTransactionNumber,
                PayTypeId = e.PayTypeId,
                PayType = e.PayType,
            };
            var CourseOrderFilterSpec = new CourseOrderByClientFilterSpecification(request.SearchString,request.ClientId);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<CourseOrder>().Entities
                   //.Specify(CourseOrderFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<CourseOrder>().Entities
                   .Specify(CourseOrderFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}