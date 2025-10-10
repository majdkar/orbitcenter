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

namespace SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged
{
    public class GetAllPagedProductOrdersQuery : IRequest<PaginatedResult<GetAllPagedProductOrdersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedProductOrdersQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllProductOrdersQueryHandler : IRequestHandler<GetAllPagedProductOrdersQuery, PaginatedResult<GetAllPagedProductOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductOrdersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductOrdersResponse>> Handle(GetAllPagedProductOrdersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductOrder, GetAllPagedProductOrdersResponse>> expression = e => new GetAllPagedProductOrdersResponse
            {
                Id = e.Id,
                Notes = e.Notes,
                TotalPrice = e.TotalPrice,
                Items = e.Items,
                ClientId = e.ClientId,
                Status = e.Status,
                PaymentStatus = e.PaymentStatus,
                OrderNumber = e.OrderNumber,
                Client = e.Client,
                OrderDate = e.OrderDate,
                 ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                 ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
                ClientType = e.ClientType,

            };
            var ProductOrderFilterSpec = new ProductOrderByCompanyFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductOrder>().Entities
                   //.Specify(ProductOrderFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductOrder>().Entities
                   .Specify(ProductOrderFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}