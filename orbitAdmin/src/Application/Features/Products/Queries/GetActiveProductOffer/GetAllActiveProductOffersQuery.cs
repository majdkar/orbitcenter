using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Queries.GetActiveProductOffer
{
    public class GetAllActiveProductOffersQuery : IRequest<List<GetAllActiveProductOffersResponse>>
    {

    }
    internal class GetAllProductsQueryHandler : IRequestHandler<GetAllActiveProductOffersQuery, List<GetAllActiveProductOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllActiveProductOffersResponse>> Handle(GetAllActiveProductOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductOffer, GetAllActiveProductOffersResponse>> expression = e => new GetAllActiveProductOffersResponse
            {
                Id = e.Id,
                ProductId = e.ProductId,
                ProductNameAr = e.Product.NameAr,
                ProductNameEn = e.Product.NameEn,
                 
                Price = e.Product.Price,
                //CompanyId = e.Product.CompanyId,
                //CompanyNameAr = e.Product.Company.NameAr,
                //CompanyNameEn = e.Product.Company.NameEn,
                OldPrice = e.Product.Price,
                NewPrice = e.NewPrice,
                DiscountRatio = e.DiscountRatio,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            };
            var offerFilterSpec = new AllActiveProductOfferFilterSpecification();
            var data = await _unitOfWork.Repository<ProductOffer>().Entities
                  .Specify(offerFilterSpec)
                  .Select(expression)
                  .ToListAsync();
            return data;
        }
    }
}