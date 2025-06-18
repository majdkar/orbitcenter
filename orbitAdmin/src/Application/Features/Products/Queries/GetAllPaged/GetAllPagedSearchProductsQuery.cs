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
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedSearchProductsQuery : IRequest<PaginatedResult<GetAllPagedProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }

        public string ProductName { get; set; }

        public int ProductCategoryId { get; set; }
        public int ProductSubCategoryId { get; set; }
        public int ProductSubSubCategoryId { get; set; }
        public int ProductSubSubSubCategoryId { get; set; }


        public decimal FromPrice { get; set; }

        public decimal ToPrice { get; set; }


        public GetAllPagedSearchProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy, string productname, int propductcategoryid,int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ProductName = productname;
            ProductCategoryId = propductcategoryid;
            ProductSubCategoryId = propductSubcategoryid;
            ProductSubSubCategoryId = propductSubSubcategoryid;
            ProductSubSubSubCategoryId = propductSubSubSubcategoryid;
            FromPrice = fromprice;
            ToPrice = toprice;
        }
    }

    internal class GetAllPagedSearchProductsQueryHandler : IRequestHandler<GetAllPagedSearchProductsQuery, PaginatedResult<GetAllPagedProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> Handle(GetAllPagedSearchProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetAllPagedProductsResponse>> expression = e => new GetAllPagedProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
         
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4,


                DescriptionEn1 = e.DescriptionEn1,
                DescriptionEn2 = e.DescriptionEn2,
                DescriptionEn3 = e.DescriptionEn3,
                DescriptionEn4 = e.DescriptionEn4,

                DescriptionGe1 = e.DescriptionGe1,
                DescriptionGe2 = e.DescriptionGe2,
                DescriptionGe3 = e.DescriptionGe3,
                DescriptionGe4 = e.DescriptionGe4,


                ProductParentCategoryId = e.ProductParentCategoryId,
                ProductSubCategoryId = e.ProductSubCategoryId,
                ProductSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductSubSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductDefaultCategoryId = e.ProductDefaultCategoryId.Value,


                Price = e.Price,
                Code = e.Code,

                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,

                NameGe = e.NameGe,
                Plan = e.Plan,
                ProductDefaultCategory = e.ProductDefaultCategory,
                ProductImageUrl1 = e.ProductImageUrl1,
                ProductImageUrl2 = e.ProductImageUrl2,
                ProductImageUrl3 = e.ProductImageUrl3,
                ProductOffers = e.ProductOffers,
                 SeoDescription = e.SeoDescription,
                  Keywords = e.Keywords,
            };
            var productFilterSpec = new ProductSearchFilterSpecification(request.SearchString, request.ProductName, request.ProductCategoryId,request.ProductSubCategoryId,request.ProductSubSubCategoryId,request.ProductSubSubSubCategoryId, request.FromPrice, request.ToPrice);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}