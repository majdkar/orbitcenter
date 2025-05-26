using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.CourseTypes.Queries
{
    public class GetAllCourseTypesQuery : IRequest<Result<List<GetAllCourseTypesResponse>>>
    {
        public GetAllCourseTypesQuery()
        {
        }
    }
    internal class GetAllCourseTypesCachedQueryHandler : IRequestHandler<GetAllCourseTypesQuery, Result<List<GetAllCourseTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCourseTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCourseTypesResponse>>> Handle(GetAllCourseTypesQuery request, CancellationToken cancellationToken)
        {
            var isArabic = CultureInfo.CurrentCulture.Name.Contains("ar");
            Func<Task<List<CourseType>>> getAllCourseTypes = () => _unitOfWork.Repository<CourseType>().Entities
            .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            var CourseTypeList = await _cache.GetOrAddAsync(nameof(CourseType) + $"-{isArabic}", getAllCourseTypes);
            var mappedCourseTypes = _mapper.Map<List<GetAllCourseTypesResponse>>(CourseTypeList);
            return await Result<List<GetAllCourseTypesResponse>>.SuccessAsync(mappedCourseTypes);
        }
    }
}
