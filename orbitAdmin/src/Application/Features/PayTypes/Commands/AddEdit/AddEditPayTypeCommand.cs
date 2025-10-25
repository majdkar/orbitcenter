using System.ComponentModel.DataAnnotations;
using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Requests;
using System.IO;
using SchoolV01.Application.Interfaces.Services;
using static SchoolV01.Shared.Constants.Permission.Permissions;

namespace SchoolV01.Application.Features.PayTypes.Commands.AddEdit
{
    public partial class AddEditPayTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required] public string NameAr { get; set; }
        [Required] public string NameEn { get; set; }
       
    }
}

internal class AddEditPayTypeCommandHandler : IRequestHandler<AddEditPayTypeCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<AddEditPayTypeCommandHandler> _localizer;
    private readonly IUnitOfWork<int> _unitOfWork;
    

    public AddEditPayTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPayTypeCommandHandler> localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
        
    }

    public async Task<Result<int>> Handle(AddEditPayTypeCommand command, CancellationToken cancellationToken)
    {
       
        if (command.Id == 0)
        {
            var PayType = _mapper.Map<PayType>(command);
            
            await _unitOfWork.Repository<PayType>().AddAsync(PayType);
            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPayTypesCacheKey);
            return await Result<int>.SuccessAsync(PayType.Id, _localizer["PayType Saved"]);
        }
        else
        {
            var PayType = await _unitOfWork.Repository<PayType>().GetByIdAsync(command.Id);
            if (PayType != null)
            {
               
                PayType.NameAr = command.NameAr ?? PayType.NameAr;
                PayType.NameEn = command.NameEn ?? PayType.NameEn;
                await _unitOfWork.Repository<PayType>().UpdateAsync(PayType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPayTypesCacheKey);
                return await Result<int>.SuccessAsync(PayType.Id, _localizer["PayType Updated"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["PayType Not Found!"]);
            }
        }
    }
}
