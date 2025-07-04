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
using SchoolV01.Application.Specifications.Courses;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.Courses.Queries.GetById
{
    public class GetCourseByIdQuery : IRequest<Result<GetCourseByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Result<GetCourseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseByIdResponse>> Handle(GetCourseByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new CourseByIdFilterSpecification(query.Id);
            Expression<Func<Course, GetCourseByIdResponse>> expression = e => new GetCourseByIdResponse
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
                 
                 Keywords = e.Keywords,
                  SeoDescription = e.SeoDescription,
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

            var Course = await _unitOfWork.Repository<Course>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetCourseByIdResponse>.SuccessAsync(Course);
        }
    }
}