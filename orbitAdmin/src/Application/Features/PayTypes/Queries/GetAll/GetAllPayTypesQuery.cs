using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.PayTypes.Queries.GetAll
{
    public class GetAllPayTypesQuery : IRequest<Result<List<GetAllPayTypesResponse>>>
    {
        public GetAllPayTypesQuery()
        {
        }
    }

    internal class GetAllPayTypesCachedQueryHandler : IRequestHandler<GetAllPayTypesQuery, Result<List<GetAllPayTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPayTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPayTypesResponse>>> Handle(GetAllPayTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<PayType>>> getAllPayTypes = () => _unitOfWork.Repository<PayType>().GetAllAsync();
            var PayTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPayTypesCacheKey, getAllPayTypes);
            var mappedPayTypes = _mapper.Map<List<GetAllPayTypesResponse>>(PayTypeList);
            return await Result<List<GetAllPayTypesResponse>>.SuccessAsync(mappedPayTypes);
        }
    }
}