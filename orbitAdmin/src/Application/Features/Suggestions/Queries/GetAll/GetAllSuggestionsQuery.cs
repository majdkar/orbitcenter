using MediatR;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using SchoolV01.Shared.Constants.Clients;

using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Domain.Entities.Suggestions;
using SchoolV01.Application.Specifications.Suggestions;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Features.Suggestions.Queries.GetAll
{
    public class GetAllSuggestionsQuery : IRequest<PaginatedResult<GetAllSuggestionsResponse>>
    {

        public SuggestionType Type { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllSuggestionsQuery(int pageNumber, int pageSize, string searchString, string orderBy, SuggestionType type)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            Type = type;

        }
    }

    internal class GetAllSuggestionsQueryHandler : IRequestHandler<GetAllSuggestionsQuery, PaginatedResult<GetAllSuggestionsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;


        public GetAllSuggestionsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public async Task<PaginatedResult<GetAllSuggestionsResponse>> Handle(GetAllSuggestionsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Suggestion, GetAllSuggestionsResponse>> expression = e => new GetAllSuggestionsResponse
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
            var NotificationsFilterSpec = new SuggestionFilterSpecification(request.SearchString,request.Type);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Suggestion>().Entities.Include(x => x.Client).ThenInclude(x => x.User)
                    .Specify(NotificationsFilterSpec)
                    .Select(expression)
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Suggestion>().Entities.Include(x => x.Client).ThenInclude(x => x.User)
                   .Specify(NotificationsFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }

    }
}