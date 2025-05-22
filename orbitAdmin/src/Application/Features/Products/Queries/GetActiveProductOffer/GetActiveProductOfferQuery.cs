using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Queries.GetActiveProductOffer
{
    public class GetActiveProductOfferQuery : IRequest<Result<GetActiveProductOfferResponse>>
    {
        public int ProductId { get; set; }
    }

    internal class GetActiveProductOfferQueryHandler : IRequestHandler<GetActiveProductOfferQuery, Result<GetActiveProductOfferResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveProductOfferQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetActiveProductOfferResponse>> Handle(GetActiveProductOfferQuery request, CancellationToken cancellationToken)
        {
            var productOffers = _unitOfWork.Repository<ProductOffer>().Entities
                .FirstOrDefault(x => x.ProductId == request.ProductId && x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date);
            var mappedOffers = _mapper.Map<GetActiveProductOfferResponse>(productOffers);
            return await Result<GetActiveProductOfferResponse>.SuccessAsync(mappedOffers);
        }
    }
}

