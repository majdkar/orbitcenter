using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Commands.Delete
{
    public class DeleteProductOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductOfferCommandHandler : IRequestHandler<DeleteProductOfferCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductOfferCommandHandler> _localizer;

        public DeleteProductOfferCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductOfferCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductOfferCommand command, CancellationToken cancellationToken)
        {
            var productOffer = await _unitOfWork.Repository<ProductOffer>().GetByIdAsync(command.Id);
            if (productOffer != null)
            {
                await _unitOfWork.Repository<ProductOffer>().DeleteAsync(productOffer);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productOffer.Id, _localizer["Product Offer Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Offer Not Found!"]);
            }
        }
    }
}