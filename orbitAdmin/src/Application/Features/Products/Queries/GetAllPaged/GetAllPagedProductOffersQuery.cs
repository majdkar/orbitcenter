using MediatR;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Domain.Entities.Products;
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
using SchoolV01.Application.Specifications.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductOffersQuery : IRequest<PaginatedResult<GetAllPagedProductOffersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public GetAllPagedProductOffersQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllProductOffersQueryHandler : IRequestHandler<GetAllPagedProductOffersQuery, PaginatedResult<GetAllPagedProductOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductOffersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductOffersResponse>> Handle(GetAllPagedProductOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductOffer, GetAllPagedProductOffersResponse>> expression = e => new GetAllPagedProductOffersResponse
            {
                Id = e.Id,
                Product = e.Product,
                Category = e.Product.ProductDefaultCategory,
                ProductId = e.ProductId,
                ProductImageUrl1 = e.Product.ProductImageUrl1,
                ProductImageUrl2 = e.Product.ProductImageUrl1,
                ProductImageUrl3 = e.Product.ProductImageUrl1,
                NameAr = e.Product.NameAr,
                NameEn = e.Product.NameEn,
                Code = e.Product.Code,

                DescriptionAr1 = e.Product.DescriptionAr1,
                DescriptionAr2 = e.Product.DescriptionAr2,
                DescriptionAr3 = e.Product.DescriptionAr3,
                DescriptionAr4 = e.Product.DescriptionAr4,
                DescriptionEn1 = e.Product.DescriptionEn1,
                DescriptionEn2 = e.Product.DescriptionEn1,
                DescriptionEn3 = e.Product.DescriptionEn1,
                DescriptionEn4 = e.Product.DescriptionEn1,

                DescriptionGe1 = e.Product.DescriptionGe1,
                DescriptionGe2 = e.Product.DescriptionGe2,
                DescriptionGe3 = e.Product.DescriptionGe3,
                DescriptionGe4 = e.Product.DescriptionGe4,


                Order = e.Product.Order,
                IsVisible = e.Product.IsVisible,
                IsRecent = e.Product.IsRecent,
                DiscountRatio = e.DiscountRatio,
                EndDate = e.EndDate,
                NewPrice = e.NewPrice,
                StartDate = e.StartDate,
                Plan = e.Product.Plan,
                Price = e.Product.Price


            };
            var productOfferFilterSpec = new ProdectOfferPagedSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductOffer>().Entities
                   .Specify(productOfferFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductOffer>().Entities
                   .Specify(productOfferFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
