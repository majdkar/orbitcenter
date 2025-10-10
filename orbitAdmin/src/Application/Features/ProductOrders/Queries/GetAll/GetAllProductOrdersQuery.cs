using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Text.Json;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Application.Specifications.Catalog;

namespace SchoolV01.Application.Features.ProductOrders.Queries.GetAll
{
    public class GetAllProductOrdersQuery : IRequest<Result<List<GetAllProductOrdersResponse>>>
    {
        public GetAllProductOrdersQuery()
        {

        }
    }
    public class GetAllProductOrdersCachedQueryHandler : IRequestHandler<GetAllProductOrdersQuery, Result<List<GetAllProductOrdersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductOrdersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductOrdersResponse>>> Handle(GetAllProductOrdersQuery request, CancellationToken cancellationToken)
        {
            var filter = new ProductOrderFilterSpecification();
            Expression<Func<ProductOrder, GetAllProductOrdersResponse>> expression =  e => new GetAllProductOrdersResponse
            {
                Id = e.Id,
                Notes =e.Notes,
               TotalPrice = e.TotalPrice,
               Items = e.Items,
                ClientId =e.ClientId,
                Status =e.Status,
                PaymentStatus =e.PaymentStatus,
                OrderNumber =e.OrderNumber,
                Client =e.Client,
                ClientType = e.ClientType,

                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
            };

            var  getAllProductOrders = await _unitOfWork.Repository<ProductOrder>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllProductOrdersResponse>>.SuccessAsync(getAllProductOrders);

        }

 



    }
}
