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

namespace SchoolV01.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllProductCategorySonsQuery : IRequest<Result<List<GetAllProductCategorySonsResponse>>>
    {
        public int Id { get; set; }
        public GetAllProductCategorySonsQuery(int productCategoryId)
        {
            Id = productCategoryId;
        }

    }
    internal class GetAllProductCategorySonsCachedQueryHandler : IRequestHandler<GetAllProductCategorySonsQuery, Result<List<GetAllProductCategorySonsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductCategorySonsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductCategorySonsResponse>>> Handle(GetAllProductCategorySonsQuery request, CancellationToken cancellationToken)
        {
            //Func<Task<List<ProductCategory>>> getAllProductCategories = () => _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            //var categoriesList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllProductCategoriesCacheKey, getAllProductCategories);
            //var mappedcategoriess = _mapper.Map<List<GetAllProductCategorySonsResponse>>(categoriesList);
            //var categorySonsList = mappedcategoriess.Where(x => x.ParentCategoryId == request.Id).ToList();
            //return await Result<List<GetAllProductCategorySonsResponse>>.SuccessAsync(categorySonsList);

            var categoriesList = _unitOfWork.Repository<ProductCategory>().Entities.Where(x => x.ParentCategoryId == request.Id);
            var mappedSons = _mapper.Map<List<GetAllProductCategorySonsResponse>>(categoriesList);
            return await Result<List<GetAllProductCategorySonsResponse>>.SuccessAsync(mappedSons);
        }
    }
}
