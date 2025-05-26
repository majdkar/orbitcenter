using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Classifications.Commands
{
    public class AddEditClassificationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }

        public string NameEn { get; set; }

    }

    internal class AddEditClassificationCommandHandler : IRequestHandler<AddEditClassificationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditClassificationCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditClassificationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClassificationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditClassificationCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var Classification = _mapper.Map<Classification>(command);
                await _unitOfWork.Repository<Classification>().AddAsync(Classification);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Classification));
                return await Result<int>.SuccessAsync(Classification.Id, _localizer["Classification Saved"]);
            }
            else
            {
                var Classification = await _unitOfWork.Repository<Classification>().GetByIdAsync(command.Id);
                if (Classification != null)
                {
                    Classification.NameAr = command.NameAr ?? Classification.NameAr;
                    Classification.NameEn = command.NameEn ?? Classification.NameEn;
                

                    await _unitOfWork.Repository<Classification>().UpdateAsync(Classification);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(Classification));
                    return await Result<int>.SuccessAsync(Classification.Id, _localizer["Classification Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Classification Not Found!"]);
                }
            }
        }
    }
}
