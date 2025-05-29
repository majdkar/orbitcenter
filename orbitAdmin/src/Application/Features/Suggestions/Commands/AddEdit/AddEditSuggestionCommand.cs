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
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Features.Suggestions.Commands.AddEdit
{
    public class AddEditSuggestionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int? ClientId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string? Reply { get; set; }
        public string Mobile { get; set; }
        public SuggestionType Type { get; set; }

    }

    internal class AddEditSuggestionCommandHandler : IRequestHandler<AddEditSuggestionCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditSuggestionCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditSuggestionCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditSuggestionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditSuggestionCommand command, CancellationToken cancellationToken)
        {

            if (command.Id == 0)
            {
                var suggestion = _mapper.Map<Suggestion>(command);
                await _unitOfWork.Repository<Suggestion>().AddAsync(suggestion);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSuggestionsCacheKey);
                return await Result<int>.SuccessAsync(suggestion.Id, _localizer["Suggestion Saved"]);
            }
            else
            {
                var suggestion = await _unitOfWork.Repository<Suggestion>().GetByIdAsync(command.Id);
                if (suggestion != null)
                {
                    suggestion.UserName = command.UserName;
                    suggestion.ClientId = command.ClientId;
                    suggestion.Email = command.Email;
                    suggestion.Type = command.Type;
                    suggestion.Description = command.Description;
                    suggestion.Reply = command.Reply;
                    suggestion.Mobile = command.Mobile;
                    await _unitOfWork.Repository<Suggestion>().UpdateAsync(suggestion);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSuggestionsCacheKey);
                    return await Result<int>.SuccessAsync(suggestion.Id, _localizer["Suggestion Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Suggestion Not Found!"]);
                }
            }
        }
    }
}
