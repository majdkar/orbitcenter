using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.CourseCategories.Queries.GetAll
{
    public class GetAllCourseCategorySonsQuery : IRequest<Result<List<GetAllCourseCategorySonsResponse>>>
    {
        public int Id { get; set; }
        public GetAllCourseCategorySonsQuery(int CourseCategoryId)
        {
            Id = CourseCategoryId;
        }

    }
    internal class GetAllCourseCategorySonsCachedQueryHandler : IRequestHandler<GetAllCourseCategorySonsQuery, Result<List<GetAllCourseCategorySonsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCourseCategorySonsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCourseCategorySonsResponse>>> Handle(GetAllCourseCategorySonsQuery request, CancellationToken cancellationToken)
        {
            //Func<Task<List<CourseCategory>>> getAllCourseCategories = () => _unitOfWork.Repository<CourseCategory>().GetAllAsync();
            //var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllCourseCategoriesCacheKey, getAllCourseCategories);
            //var mappedcategoriess = _mapper.Map<List<GetAllCourseCategorySonsResponse>>(categoriesList);
            //var categorySonsList = mappedcategoriess.Where(x => x.ParentCategoryId == request.Id).ToList();
            //return await Result<List<GetAllCourseCategorySonsResponse>>.SuccessAsync(categorySonsList);

            var categoriesList = _unitOfWork.Repository<CourseCategory>().Entities.Where(x => x.ParentCategoryId == request.Id);
            var mappedSons = _mapper.Map<List<GetAllCourseCategorySonsResponse>>(categoriesList);
            return await Result<List<GetAllCourseCategorySonsResponse>>.SuccessAsync(mappedSons);
        }
    }
}
