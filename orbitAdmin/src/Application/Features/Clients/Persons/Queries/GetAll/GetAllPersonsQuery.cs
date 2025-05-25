﻿using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ClientNameSpace = SchoolV01.Domain.Entities.Clients;

namespace SchoolV01.Application.Features.Clients.Persons.Queries.GetAll
{
    public class GetAllPersonsQuery : IRequest<Result<List<GetAllPersonsResponse>>>
    {
        public GetAllPersonsQuery()
        {

        }
    }
    internal class GetAllPersonsCachedQueryHandler : IRequestHandler<GetAllPersonsQuery, Result<List<GetAllPersonsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPersonsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPersonsResponse>>> Handle(GetAllPersonsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Person>>> getAllPersons = () => _unitOfWork.Repository<Person>().GetAllAsync();
            var personsList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPersonsCacheKey, getAllPersons);
            var mappedPersons = _mapper.Map<List<GetAllPersonsResponse>>(personsList);
            return await Result<List<GetAllPersonsResponse>>.SuccessAsync(mappedPersons);
        }
    }
}