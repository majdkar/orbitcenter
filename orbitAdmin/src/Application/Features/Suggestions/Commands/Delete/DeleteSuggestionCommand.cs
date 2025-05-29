using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SchoolV01.Domain.Entities.Suggestions;

namespace SchoolV01.Application.Features.Suggestions.Commands.Delete
{
    public class DeleteSuggestionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteSuggestionCommandHandler : IRequestHandler<DeleteSuggestionCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteSuggestionCommandHandler> _localizer;

        public DeleteSuggestionCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteSuggestionCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteSuggestionCommand command, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Repository<Suggestion>().GetByIdAsync(command.Id);
            
            if (order != null)
            {
                order.Deleted = true;
               
                await _unitOfWork.Repository<Suggestion>().UpdateAsync(order);
               
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllSuggestionsCacheKey);
                return await Result<int>.SuccessAsync(order.Id, _localizer["Suggestion Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Suggestion Not Found!"]);
            }
        }
    }
}
