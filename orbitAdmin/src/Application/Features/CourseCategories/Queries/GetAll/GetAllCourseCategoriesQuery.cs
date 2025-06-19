using AutoMapper;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.CourseCategories.Queries.GetAll
{
    public record GetAllCourseCategoriesQuery() : IRequest<Result<List<GetAllCourseCategoriesResponse>>>;

    internal class GetAllCourseCategoriesCachedQueryHandler : IRequestHandler<GetAllCourseCategoriesQuery, Result<List<GetAllCourseCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCourseCategoriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCourseCategoriesResponse>>> Handle(GetAllCourseCategoriesQuery request, CancellationToken cancellationToken)
        {
            var getAllCourseCategories = await _unitOfWork.Repository<CourseCategory>()
                .Entities
                .AsNoTracking()
                .ToListAsync();
            //.Where(e=> !e.IsDeleted);
            //var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCourseCategoriesCacheKey, getAllCourseCategories);
            var mappedcategoriess = _mapper.Map<List<GetAllCourseCategoriesResponse>>(getAllCourseCategories);

            return await Result<List<GetAllCourseCategoriesResponse>>.SuccessAsync(mappedcategoriess);
        }
    }
}