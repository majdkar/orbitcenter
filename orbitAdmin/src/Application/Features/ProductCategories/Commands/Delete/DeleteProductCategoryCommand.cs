using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.ProductCategories.Commands.Delete
{
    public class DeleteProductCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductCategoryCommandHandler> _localizer;

        public DeleteProductCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductCategoryCommand command, CancellationToken cancellationToken)
        {
            var productCategory = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(command.Id);
            if (productCategory != null)
            {
                productCategory.Deleted = true;
                await _unitOfWork.Repository<ProductCategory>().DeleteAsync(productCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllProductCategoriesCacheKey);
                return await Result<int>.SuccessAsync(productCategory.Id, _localizer["Product Category Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Category Not Found!"]);
            }
        }
    }
}
