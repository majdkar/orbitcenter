﻿using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;

        public DeleteProductCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
            if (product != null)
            {
                product.Deleted = true;
                await _unitOfWork.Repository<Product>().DeleteAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
            }
        }
    }
}