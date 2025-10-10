using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.ProductOrders.Commands.Delete
{
    public class DeleteProductOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductOrderCommandHandler : IRequestHandler<DeleteProductOrderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductOrderCommandHandler> _localizer;

        public DeleteProductOrderCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductOrderCommand command, CancellationToken cancellationToken)
        {
            var ProductOrder = await _unitOfWork.Repository<ProductOrder>().GetByIdAsync(command.Id);
            if (ProductOrder != null)
            {
                ProductOrder.Deleted = true;
                await _unitOfWork.Repository<ProductOrder>().UpdateAsync(ProductOrder);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(ProductOrder.Id, _localizer["ProductOrder Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["ProductOrder Not Found!"]);
            }
        }
    }
}