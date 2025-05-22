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
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Products.Queries.GetById
{
    public class GetProductOfferByIdQuery : IRequest<Result<GetProductOfferByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductOfferByIdQueryHandler : IRequestHandler<GetProductOfferByIdQuery, Result<GetProductOfferByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductOfferByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductOfferByIdResponse>> Handle(GetProductOfferByIdQuery query, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Repository<ProductOffer>().GetByIdAsync(query.Id);
            var mappedproperty = _mapper.Map<GetProductOfferByIdResponse>(property);
            //var product = _unitOfWork.Repository<Product>().GetByIdAsync(mappedproperty.ProductId).Result;
            //mappedproperty.Price = product.Price;
            //mappedproperty.Weight = product.Weight;
            //if (product.UnitId.HasValue && product.UnitId.Value > 0)
            //    mappedproperty.Unit = _unitOfWork.Repository<SchoolV01.Domain.Entities.GeneralSettings.Unit>().GetByIdAsync(product.UnitId.Value).Result;
            //if (product.CurrencyId.HasValue && product.CurrencyId.Value > 0)
            //    mappedproperty.Currency = _unitOfWork.Repository<Currency>().GetByIdAsync(product.CurrencyId.Value).Result;
            //if (mappedproperty.ProductWeightId.HasValue && mappedproperty.ProductWeightId.Value > 0)
            //{

            //    mappedproperty.ProductWeight = _unitOfWork.Repository<ProductWeight>().GetByIdAsync(mappedproperty.ProductWeightId.Value).Result;
            //    if (mappedproperty.ProductWeight.UnitId > 0)
            //        mappedproperty.ProductWeight.Unit = _unitOfWork.Repository<SchoolV01.Domain.Entities.GeneralSettings.Unit>().GetByIdAsync(mappedproperty.ProductWeight.UnitId).Result;
            //    if (mappedproperty.ProductWeight.CurrencyId > 0)
            //        mappedproperty.ProductWeight.Currency = _unitOfWork.Repository<Currency>().GetByIdAsync(mappedproperty.ProductWeight.CurrencyId).Result;

            //}
            return await Result<GetProductOfferByIdResponse>.SuccessAsync(mappedproperty);
        }
    }
}