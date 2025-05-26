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

namespace SchoolV01.Application.Features.Classifications.Queries
{
    public class GetAllClassificationsQuery : IRequest<Result<List<GetAllClassificationsResponse>>>
    {
        public GetAllClassificationsQuery()
        {
        }
    }
    internal class GetAllClassificationsCachedQueryHandler : IRequestHandler<GetAllClassificationsQuery, Result<List<GetAllClassificationsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllClassificationsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllClassificationsResponse>>> Handle(GetAllClassificationsQuery request, CancellationToken cancellationToken)
        {
            var isArabic = CultureInfo.CurrentCulture.Name.Contains("ar");
            Func<Task<List<Country>>> getAllClassifications = () => _unitOfWork.Repository<Country>().Entities
            .OrderBy(x => isArabic ? x.NameAr : x.NameEn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            var CountryList = await _cache.GetOrAddAsync(nameof(Country) + $"-{isArabic}", getAllClassifications);
            var mappedClassifications = _mapper.Map<List<GetAllClassificationsResponse>>(CountryList);
            return await Result<List<GetAllClassificationsResponse>>.SuccessAsync(mappedClassifications);
        }
    }
}
