﻿using AutoMapper;
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
    public class GetCourseByEndpointQuery : IRequest<Result<GetCourseByIdResponse>>
    {
        public string Endpoint { get; set; }
    }

    internal class GetCourseByEndpointQueryHandler : IRequestHandler<GetCourseByEndpointQuery, Result<GetCourseByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseByEndpointQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseByIdResponse>> Handle(GetCourseByEndpointQuery query, CancellationToken cancellationToken)
        {
            var filter = new CourseByEndpointFilterSpecification(query.Endpoint);
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
                CourseSubCategoryId = e.CourseSubCategoryId,
                CourseSubSubCategoryId = e.CourseSubSubSubCategoryId,
                CourseSubSubSubCategoryId = e.CourseSubSubSubCategoryId,
                CourseDefaultCategoryId = e.CourseDefaultCategoryId.Value,


                Price = e.Price,
                Code = e.Code,

                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,

                EndpointAr = e.EndpointAr,
                EndpointGe = e.EndpointGe,
                EndpointEn = e.EndpointEn,

                NameGe = e.NameGe,
                Plan = e.Plan,
                CourseDefaultCategory = e.CourseDefaultCategory,
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
                 CourseSeos = e.CourseSeo,
            };

            var Course = await _unitOfWork.Repository<Course>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetCourseByIdResponse>.SuccessAsync(Course);
        }
    }
}