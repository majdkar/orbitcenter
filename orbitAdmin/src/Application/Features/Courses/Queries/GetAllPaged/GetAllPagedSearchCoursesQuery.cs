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
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.Courses.Queries.GetAllPaged
{
    public class GetAllPagedSearchCoursesQuery : IRequest<PaginatedResult<GetAllPagedCoursesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }

        public string CourseName { get; set; }

        public int CourseCategoryId { get; set; }
        public int CourseSubCategoryId { get; set; }
        public int CourseSubSubCategoryId { get; set; }
        public int CourseSubSubSubCategoryId { get; set; }


        public decimal FromPrice { get; set; }

        public decimal ToPrice { get; set; }


        public GetAllPagedSearchCoursesQuery(int pageNumber, int pageSize, string searchString, string orderBy, string Coursename, int propductcategoryid,int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            CourseName = Coursename;
            CourseCategoryId = propductcategoryid;
            CourseSubCategoryId = propductSubcategoryid;
            CourseSubSubCategoryId = propductSubSubcategoryid;
            CourseSubSubSubCategoryId = propductSubSubSubcategoryid;
            FromPrice = fromprice;
            ToPrice = toprice;
        }
    }

    internal class GetAllPagedSearchCoursesQueryHandler : IRequestHandler<GetAllPagedSearchCoursesQuery, PaginatedResult<GetAllPagedCoursesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedSearchCoursesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> Handle(GetAllPagedSearchCoursesQuery request, CancellationToken cancellationToken)
        {

            var productCategory = await _unitOfWork.Repository<CourseCategory>().Entities.ToListAsync();

            Expression<Func<Course, GetAllPagedCoursesResponse>> expression = e => new GetAllPagedCoursesResponse
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


                CourseParentCategoryId = e.CourseParentCategoryId,
                CourseParentCategoryNameAr = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseParentCategoryId).NameAr,
                CourseParentCategoryNameEn = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseParentCategoryId).NameEn,
                CourseParentCategoryNameGe = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseParentCategoryId).NameGe,


                CourseSubCategoryId = e.CourseSubCategoryId,
                CourseSubCategoryNameAr = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubCategoryId).NameAr,
                CourseSubCategoryNameEn = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubCategoryId).NameEn,
                CourseSubCategoryNameGe = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubCategoryId).NameGe,



                CourseSubSubCategoryId = e.CourseSubSubCategoryId,
                CourseSubSubCategoryNameAr = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubCategoryId).NameAr,
                CourseSubSubCategoryNameEn = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubCategoryId).NameEn,
                CourseSubSubCategoryNameGe = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubCategoryId).NameGe,



                CourseSubSubSubCategoryId = e.CourseSubSubSubCategoryId,
                CourseSubSubSubCategoryNameAr = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubSubCategoryId).NameAr,
                CourseSubSubSubCategoryNameEn = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubSubCategoryId).NameEn,
                CourseSubSubSubCategoryNameGe = _unitOfWork.Repository<CourseCategory>().Entities.FirstOrDefault(x => x.Id == e.CourseSubSubSubCategoryId).NameGe,






                CourseDefaultCategoryId = e.CourseDefaultCategoryId.Value,
                CourseDefaultCategory = e.CourseDefaultCategory,


                Price = e.Price,
                Code = e.Code,

                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,

                NameGe = e.NameGe,
                Plan = e.Plan,
                CourseImageUrl1 = e.CourseImageUrl1,
                CourseImageUrl2 = e.CourseImageUrl2,
                CourseImageUrl3 = e.CourseImageUrl3,
                CourseOffers = e.CourseOffers,
                SeoDescription = e.SeoDescription,
                Keywords = e.Keywords,

                CourseTypeId = e.CourseTypeId,
                NumSesstions = e.NumSesstions,
                StartDate = e.StartDate,
                TeacherNameEn = e.TeacherNameEn,
                TeacherNameAr = e.TeacherNameAr,
                TeacherNameGe = e.TeacherNameGe,
                StartEnd = e.StartEnd,
                NumMaxStudent = e.NumMaxStudent,
                CourseType = e.CourseType,

            };
            var CourseFilterSpec = new CourseSearchFilterSpecification(request.SearchString, request.CourseName, request.CourseCategoryId,request.CourseSubCategoryId,request.CourseSubSubCategoryId,request.CourseSubSubSubCategoryId, request.FromPrice, request.ToPrice);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Course>().Entities
                   .Specify(CourseFilterSpec)
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Course>().Entities
                   .Specify(CourseFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression).AsNoTracking()
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}