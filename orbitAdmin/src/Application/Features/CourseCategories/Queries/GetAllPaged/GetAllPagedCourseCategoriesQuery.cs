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
using SchoolV01.Domain.Entities.Courses;

namespace SchoolV01.Application.Features.CourseCategories.Queries.GetAllPaged
{
    public class GetAllPagedCourseCategoriesQuery : IRequest<PaginatedResult<GetAllPagedCourseCategoriesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }

        public GetAllPagedCourseCategoriesQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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
    internal class GetAllCourseCategoriesQueryHandler : IRequestHandler<GetAllPagedCourseCategoriesQuery, PaginatedResult<GetAllPagedCourseCategoriesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCourseCategoriesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> Handle(GetAllPagedCourseCategoriesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CourseCategory, GetAllPagedCourseCategoriesResponse>> expression = e => new GetAllPagedCourseCategoriesResponse
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
                SonsCount = _unitOfWork.Query<CourseCategory>().Where(x => x.ParentCategoryId == e.Id).Count(),


            };
            var CourseCategoryFilterSpec = new CourseCategoryFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<CourseCategory>().Entities
                   .Specify(CourseCategoryFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<CourseCategory>().Entities
                   .Specify(CourseCategoryFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
