using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Commands.Delete
{
    public class DeleteProductSeoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductSeoCommandHandler : IRequestHandler<DeleteProductSeoCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductSeoCommandHandler> _localizer;

        public DeleteProductSeoCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductSeoCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductSeoCommand command, CancellationToken cancellationToken)
        {
            var productSeo = await _unitOfWork.Repository<ProductSeo>().GetByIdAsync(command.Id);
            if (productSeo != null)
            {
                await _unitOfWork.Repository<ProductSeo>().DeleteAsync(productSeo);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productSeo.Id, _localizer["Product Seo Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Seo Not Found!"]);
            }
        }
    }
}