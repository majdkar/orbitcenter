using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Core.Entities;

namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedRecentProductsQuery : IRequest<PaginatedResult<GetAllPagedRecentProductsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public GetAllPagedRecentProductsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllPagedRecentProductsQueryHandler : IRequestHandler<GetAllPagedRecentProductsQuery, PaginatedResult<GetAllPagedRecentProductsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedRecentProductsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedRecentProductsResponse>> Handle(GetAllPagedRecentProductsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Product, GetAllPagedRecentProductsResponse>> expression = e => new GetAllPagedRecentProductsResponse
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
                ProductParentCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameAr,
                ProductParentCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameEn,
                ProductParentCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameGe,


                ProductSubCategoryId = e.ProductSubCategoryId,
                ProductSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameAr,
                ProductSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameEn,
                ProductSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameGe,



                ProductSubSubCategoryId = e.ProductSubSubCategoryId,
                ProductSubSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameAr,
                ProductSubSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameEn,
                ProductSubSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameGe,



                ProductSubSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductSubSubSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameAr,
                ProductSubSubSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameEn,
                ProductSubSubSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameGe,

                ProductDefaultCategoryId = e.ProductDefaultCategoryId.Value,


                Price = e.Price,
                Code = e.Code,
                EndpointAr = e.EndpointAr,
                EndpointEn = e.EndpointEn,
                EndpointGe = e.EndpointGe,
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
            };
            var productFilterSpec = new RecentProductsFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Product>().Entities
                   .Specify(productFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
