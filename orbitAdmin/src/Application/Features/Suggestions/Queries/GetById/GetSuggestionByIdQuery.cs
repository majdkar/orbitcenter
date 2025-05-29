using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using SchoolV01.Application.Specifications.Suggestions;
using SchoolV01.Domain.Entities.Suggestions;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using System.Linq.Expressions;
using System;

namespace SchoolV01.Application.Features.Suggestions.Queries.GetById
{
    public class GetSuggestionByIdQuery : IRequest<Result<GetSuggestionByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetSuggestionByIdQueryHandler : IRequestHandler<GetSuggestionByIdQuery, Result<GetSuggestionByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetSuggestionByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetSuggestionByIdResponse>> Handle(GetSuggestionByIdQuery query, CancellationToken cancellationToken)
        {

            Expression<Func<Suggestion, GetSuggestionByIdResponse>> expression = e => new GetSuggestionByIdResponse
            {
                Id = e.Id,
                UserName = e.UserName,
                Email = e.Email,
                Description = e.Description,
                Reply = e.Reply,
                Mobile = e.Mobile,
                ClientId = e.ClientId,
                UserId = e.Client.UserId,
                Client = e.Client,
                 Type = e.Type,
                  CreateOn = e.CreatedOn,
            };

            var SuggestionsFilterSpec = new SuggestionByIdFilterSpecification(query.Id);
            var Suggestions = await _unitOfWork.Repository<Suggestion>().Entities.Include(x => x.Client).ThenInclude(x => x.User)
                .Specify(SuggestionsFilterSpec)
                .Select(expression)
                .FirstOrDefaultAsync();
            var mappedNotification = _mapper.Map<GetSuggestionByIdResponse>(Suggestions);
            
            return await Result<GetSuggestionByIdResponse>.SuccessAsync(mappedNotification);
        }
    }
}
