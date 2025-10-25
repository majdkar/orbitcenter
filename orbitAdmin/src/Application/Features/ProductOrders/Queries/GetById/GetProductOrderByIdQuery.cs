using AutoMapper;
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
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Application.Specifications.Catalog;

namespace SchoolV01.Application.Features.ProductOrders.Queries.GetById
{
    public class GetProductOrderByIdQuery : IRequest<Result<GetProductOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductOrderByIdQueryHandler : IRequestHandler<GetProductOrderByIdQuery, Result<GetProductOrderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductOrderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


     

        public async Task<Result<GetProductOrderByIdResponse>> Handle(GetProductOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new ProductOrderByIdFilterSpecification(query.Id);
            Expression<Func<ProductOrder, GetProductOrderByIdResponse>> expression = e => new GetProductOrderByIdResponse
            {
                Id = e.Id,
                Notes = e.Notes,
                TotalPrice = e.TotalPrice,

                Items = e.Items.Select(i => new ProductOrderItem
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                     Product = i.Product,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),

                ClientId = e.ClientId,
                Status = e.Status,
                 ClientType= e.ClientType,
                PaymentStatus = e.PaymentStatus,
                OrderNumber = e.OrderNumber,
                Client = e.Client,
                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
                PaymentTransactionNumber = e.PaymentTransactionNumber,
                PayTypeId = e.PayTypeId,
                PayType = e.PayType,
            };

            var ProductOrder = await _unitOfWork.Repository<ProductOrder>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetProductOrderByIdResponse>.SuccessAsync(ProductOrder);
        }
    }
}