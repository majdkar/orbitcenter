using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Shared.Constants.Application;

using SchoolV01.Shared.Wrapper;
using System;
using System.Threading;
using System.Threading.Tasks;

using SchoolV01.Domain.Entities.Suggestions;
using System.Linq;

namespace SchoolV01.Application.Features.Suggestions.Commands.AddEdit
{
    public class AddEditReplyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
     
        public string? Reply { get; set; }

    }

    internal class AddEditReplyCommandHandler : IRequestHandler<AddEditReplyCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditReplyCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditReplyCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditReplyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditReplyCommand command, CancellationToken cancellationToken)
        {

            var suggestion = _unitOfWork.Repository<Suggestion>().Entities.Where(x => x.Id == command.Id).FirstOrDefault();

            if(suggestion != null) 
            {
                suggestion.Reply = command.Reply;
                await _unitOfWork.Repository<Suggestion>().UpdateAsync(suggestion);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSuggestionsCacheKey);
                return await Result<int>.SuccessAsync(suggestion.Id, _localizer["Reply Saved"]);
            }
            else 
            {
                return await Result<int>.FailAsync(_localizer["Suggestion Not Found"]);

            }


        }
    }
}
