using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.Classifications.Commands
{
    public class DeleteClassificationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteClassificationCommandHandler : IRequestHandler<DeleteClassificationCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteClassificationCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteClassificationCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClassificationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClassificationCommand command, CancellationToken cancellationToken)
        {
            var Classification = await _unitOfWork.Repository<Classification>().Entities.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (Classification != null)
            {
                await _unitOfWork.Repository<Classification>().DeleteAsync(Classification);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Classification));
                return await Result<int>.SuccessAsync(Classification.Id, _localizer["Classification Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Classification Not Found!"]);
            }
        }
    }
}
