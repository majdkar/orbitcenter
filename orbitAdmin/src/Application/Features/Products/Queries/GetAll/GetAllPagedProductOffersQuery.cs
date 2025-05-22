using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
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
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Specifications.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllPagedProductOffersQuery : IRequest<PaginatedResult<GetAllProductOffersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public int ProductId { get; set; }
        public GetAllPagedProductOffersQuery(int productId,int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ProductId = productId;
        }
    }

    internal class GetAllPagedProductOffersQueryHandler : IRequestHandler<GetAllPagedProductOffersQuery, PaginatedResult<GetAllProductOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedProductOffersQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllProductOffersResponse>> Handle(GetAllPagedProductOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductOffer, GetAllProductOffersResponse>> expression = e => new GetAllProductOffersResponse
            {
                Id = e.Id,
                //Product = e.Product,
                ProductId = e.ProductId,
                DiscountRatio = e.DiscountRatio,
                EndDate = e.EndDate,
                NewPrice = e.NewPrice,
                StartDate = e.StartDate,
                  
                //ParentCategory=e.Product.ProductCategory.ParentCategory,
                //ParentCategoryId=e.Product.ProductCategory.ParentCategoryId



            };
            var productOfferFilterSpec = new ProdectOfferSpecification(request.ProductId, request.SearchString);
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