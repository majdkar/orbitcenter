using SchoolV01.Shared.Wrapper;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Requests.Suggestions;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Client.Infrastructure.Managers.Suggestions
{
    public interface ISuggestionManager : IManager
    {
        Task<PaginatedResult<GetAllSuggestionsResponse>> GetAllPagedAsync(GetAllPagedSuggestionRequest request,SuggestionType type);

        Task<IResult<int>> SaveAsync(AddEditSuggestionCommand request);

        Task<IResult<int>> SaveReplyAsync(AddEditReplyCommand request);


        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<GetSuggestionByIdResponse>> GetByIdAsync(int SuggestionId);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "", SuggestionType type = 0);
    }
}