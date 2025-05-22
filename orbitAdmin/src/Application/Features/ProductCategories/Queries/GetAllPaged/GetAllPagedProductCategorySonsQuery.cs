using MediatR;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Wrapper;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;


namespace SchoolV01.Application.Features.ProductCategories.Queries.GetAllPaged
{
    public class GetAllPagedProductCategorySonsQuery : IRequest<PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }
        public int CategoryId { get; set; }

        public GetAllPagedProductCategorySonsQuery(int categoryId, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            CategoryId = categoryId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
    internal class GetAllProductCategorySonsQueryHandler : IRequestHandler<GetAllPagedProductCategorySonsQuery, PaginatedResult<GetAllPagedProductCategoriesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllProductCategorySonsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> Handle(GetAllPagedProductCategorySonsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductCategory, GetAllPagedProductCategoriesResponse>> expression = e => new GetAllPagedProductCategoriesResponse
            {
                Id = e.Id,
                NameEn = e.NameEn,
                NameAr = e.NameAr,
                DescriptionEn1 = e.DescriptionEn1,
                DescriptionEn2 = e.DescriptionEn2,
                DescriptionEn3 = e.DescriptionEn3,
                DescriptionEn4 = e.DescriptionEn4,
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4,
                DescriptionGe1 = e.DescriptionGe1,
                DescriptionGe2 = e.DescriptionGe2,
                DescriptionGe3 = e.DescriptionGe3,
                DescriptionGe4 = e.DescriptionGe4,
                ParentCategoryId = e.ParentCategoryId,
                ParentCategory = e.ParentCategory,
                Order = e.Order,
                NameGe = e.NameGe,
                ImageDataURL1 = e.ImageDataURL1,
                ImageDataURL2 = e.ImageDataURL2,
                ImageDataURL3 = e.ImageDataURL3,
                SonsCount = _unitOfWork.Query<ProductCategory>().Where(x => x.ParentCategoryId == e.Id).Count(),


            };
            var productCategoryFilterSpec = new ProductCategoryByParentCategoryFilterSpecification(request.SearchString, request.CategoryId);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductCategory>().Entities
                   .Specify(productCategoryFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductCategory>().Entities
                   .Specify(productCategoryFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}

