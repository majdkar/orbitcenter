using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Text.Json;
using SchoolV01.Application.Specifications.Courses;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCoursesQuery : IRequest<Result<List<GetAllCoursesResponse>>>
    {
        public GetAllCoursesQuery()
        {

        }
    }
    public class GetAllCoursesCachedQueryHandler : IRequestHandler<GetAllCoursesQuery, Result<List<GetAllCoursesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCoursesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCoursesResponse>>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var filter = new CourseFilterSpecification();
            Expression<Func<Course, GetAllCoursesResponse>> expression =  e => new GetAllCoursesResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4, 
                
                
                DescriptionEn1 = e.DescriptionAr1,
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
                EndpointAr = e.EndpointAr,
                EndpointGe = e.EndpointGe,
                EndpointEn = e.EndpointEn,
                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,
                
                NameGe = e.NameGe,
                Plan = e.Plan,
                CourseDefaultCategory = e.CourseDefaultCategory,
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

            var  getAllCourses = await _unitOfWork.Repository<Course>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllCoursesResponse>>.SuccessAsync(getAllCourses);

        }

 



    }
}
