using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductSeosQuery : IRequest<Result<List<GetAllProductSeosResponse>>>
    {
        public int ProductId { get; set; }
    }

    internal class GetAllProductSeosQueryHandler : IRequestHandler<GetAllProductSeosQuery, Result<List<GetAllProductSeosResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductSeosQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProductSeosResponse>>> Handle(GetAllProductSeosQuery request, CancellationToken cancellationToken)
        {

            var productSeos = _unitOfWork.Repository<ProductSeo>().Entities.Where(x => x.ProductId == request.ProductId);
            var mappedSeos = _mapper.Map<List<GetAllProductSeosResponse>>(productSeos);
         

            return await Result<List<GetAllProductSeosResponse>>.SuccessAsync(mappedSeos);
        }
    }
}