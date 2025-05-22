using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
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

namespace SchoolV01.Application.Features.Products.Commands.AddEdit
{
    public class AddEditProductOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal? DiscountRatio { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? NewPrice { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

internal class AddEditProductOfferCommandHandler : IRequestHandler<AddEditProductOfferCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditProductOfferCommandHandler> _localizer;
    private readonly IUnitOfWork<int> _unitOfWork;

    public AddEditProductOfferCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditProductOfferCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
    }

    public async Task<Result<int>> Handle(AddEditProductOfferCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == 0)
        {

                var productOffer = _mapper.Map<ProductOffer>(command);

           
            await _unitOfWork.Repository<ProductOffer>().AddAsync(productOffer);
                try
                {
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductOffersCacheKey);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                return await Result<int>.SuccessAsync(productOffer.Id, _localizer["Product Offer Saved"]);
            
        }
        else
        {
            var productOffer = await _unitOfWork.Repository<ProductOffer>().GetByIdAsync(command.Id);
            if (productOffer != null)
            {
                productOffer.NewPrice = command.NewPrice ?? productOffer.NewPrice;
                productOffer.DiscountRatio = command.DiscountRatio ?? productOffer.DiscountRatio;
                productOffer.StartDate = command.StartDate ?? productOffer.StartDate;
                productOffer.EndDate = command.EndDate ?? productOffer.EndDate;
                await _unitOfWork.Repository<ProductOffer>().UpdateAsync(productOffer);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductOffersCacheKey);
                return await Result<int>.SuccessAsync(productOffer.Id, _localizer["Product Offer Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Offer Not Found!"]);
            }
        }
    }
}
