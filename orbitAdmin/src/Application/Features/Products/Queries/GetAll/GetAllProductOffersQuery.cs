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
    public class GetAllProductOffersQuery : IRequest<Result<List<GetAllProductOffersResponse>>>
    {
        public int ProductId { get; set; }
    }

    internal class GetAllProductOffersQueryHandler : IRequestHandler<GetAllProductOffersQuery, Result<List<GetAllProductOffersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductOffersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProductOffersResponse>>> Handle(GetAllProductOffersQuery request, CancellationToken cancellationToken)
        {

            var productOffers = _unitOfWork.Repository<ProductOffer>().Entities.Where(x => x.ProductId == request.ProductId);
            var mappedOffers = _mapper.Map<List<GetAllProductOffersResponse>>(productOffers);
         

            return await Result<List<GetAllProductOffersResponse>>.SuccessAsync(mappedOffers);
        }
    }
}