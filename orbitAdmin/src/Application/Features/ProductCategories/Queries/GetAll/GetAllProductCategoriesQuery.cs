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

namespace SchoolV01.Application.Features.ProductCategories.Queries.GetAll
{
    public record GetAllProductCategoriesQuery() : IRequest<Result<List<GetAllProductCategoriesResponse>>>;

    internal class GetAllProductCategoriesCachedQueryHandler : IRequestHandler<GetAllProductCategoriesQuery, Result<List<GetAllProductCategoriesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductCategoriesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductCategoriesResponse>>> Handle(GetAllProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            var getAllProductCategories = await _unitOfWork.Repository<ProductCategory>()
                .Entities
                .AsNoTracking()
                .ToListAsync();
            //.Where(e=> !e.IsDeleted);
            //var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            var mappedcategoriess = _mapper.Map<List<GetAllProductCategoriesResponse>>(getAllProductCategories);

            return await Result<List<GetAllProductCategoriesResponse>>.SuccessAsync(mappedcategoriess);
        }
    }
}