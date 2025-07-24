using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
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
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Text.Json;
using SchoolV01.Application.Specifications.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllEndpointProductsQuery : IRequest<Result<List<GetAllEndpointProductsResponse>>>
    {
        public GetAllEndpointProductsQuery()
        {

        }
    }
    public class GetAllEndpointProductsCachedQueryHandler : IRequestHandler<GetAllEndpointProductsQuery, Result<List<GetAllEndpointProductsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllEndpointProductsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllEndpointProductsResponse>>> Handle(GetAllEndpointProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = new ProductFilterSpecification();
            Expression<Func<Product, GetAllEndpointProductsResponse>> expression =  e => new GetAllEndpointProductsResponse
            {
                Id = e.Id,
            

                EndpointAr = e.EndpointAr,
                EndpointEn = e.EndpointEn,
                EndpointGe = e.EndpointGe,

            };

            var  getAllProducts = await _unitOfWork.Repository<Product>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllEndpointProductsResponse>>.SuccessAsync(getAllProducts);

        }

 



    }
}
