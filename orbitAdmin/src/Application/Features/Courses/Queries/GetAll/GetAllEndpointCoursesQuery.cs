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
    public class GetAllEndpointCoursesQuery : IRequest<Result<List<GetAllEndpointCoursesResponse>>>
    {
        public GetAllEndpointCoursesQuery()
        {

        }
    }
    public class GetAllEndpointCoursesCachedQueryHandler : IRequestHandler<GetAllEndpointCoursesQuery, Result<List<GetAllEndpointCoursesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllEndpointCoursesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllEndpointCoursesResponse>>> Handle(GetAllEndpointCoursesQuery request, CancellationToken cancellationToken)
        {
            var filter = new CourseFilterSpecification();
            Expression<Func<Course, GetAllEndpointCoursesResponse>> expression =  e => new GetAllEndpointCoursesResponse
            {
                Id = e.Id,



                EndpointAr = e.EndpointAr,
                EndpointGe = e.EndpointGe,
                EndpointEn = e.EndpointEn,
              

            };

            var  getAllCourses = await _unitOfWork.Repository<Course>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllEndpointCoursesResponse>>.SuccessAsync(getAllCourses);

        }

 



    }
}
