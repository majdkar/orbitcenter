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

namespace SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged
{
    public class GetAllPagedSearchProductOrdersQuery : IRequest<PaginatedResult<GetAllPagedProductOrdersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }

        public string OrderNumber { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }

        public decimal FromPrice { get; set; }

        public decimal ToPrice { get; set; }


        public GetAllPagedSearchProductOrdersQuery(int pageNumber, int pageSize, string searchString, string orderBy, string orderNumber, int clientId, int productId, decimal fromprice, decimal toprice)
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
            ProductId = productId;
            FromPrice = fromprice;
            ToPrice = toprice;
        }
    }

    internal class GetAllPagedSearchProductOrdersQueryHandler : IRequestHandler<GetAllPagedSearchProductOrdersQuery, PaginatedResult<GetAllPagedProductOrdersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchProductOrdersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductOrdersResponse>> Handle(GetAllPagedSearchProductOrdersQuery request, CancellationToken cancellationToken)
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
                ClientType = e.ClientType,

                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,

                PaymentTransactionNumber = e.PaymentTransactionNumber,
                PayTypeId = e.PayTypeId,
                PayType = e.PayType,
            };
            var ProductOrderFilterSpec = new ProductOrderSearchFilterSpecification(request.SearchString, request.OrderNumber, request.ProductId, request.ClientId, request.FromPrice, request.ToPrice);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductOrder>().Entities
                   .Specify(ProductOrderFilterSpec)
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductOrder>().Entities
                   .Specify(ProductOrderFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}