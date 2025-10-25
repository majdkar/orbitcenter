using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.PayTypes.Commands.Delete
{
    public class DeletePayTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePayTypeCommandHandler : IRequestHandler<DeletePayTypeCommand, Result<int>>
    {
        
        private readonly IStringLocalizer<DeletePayTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePayTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePayTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePayTypeCommand command, CancellationToken cancellationToken)
        {
            var PayType = await _unitOfWork.Repository<PayType>().GetByIdAsync(command.Id);
            if (PayType != null)
            {
                PayType.Deleted = true;
                await _unitOfWork.Repository<PayType>().UpdateAsync(PayType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPayTypesCacheKey);
                return await Result<int>.SuccessAsync(PayType.Id, _localizer["PayType Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["PayType Not Found!"]);
            }
        }
    }
}