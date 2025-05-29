using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using SchoolV01.Application.Requests.Suggestions;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Client.Infrastructure.Managers.Suggestions
{
    public class SuggestionManager : ISuggestionManager
    {
        private readonly HttpClient _httpClient;

        public SuggestionManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "", SuggestionType type = 0)
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.SuggestionEndpoints.Export
                : Routes.SuggestionEndpoints.ExportFiltered(searchString,type));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.SuggestionEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllSuggestionsResponse>> GetAllPagedAsync(GetAllPagedSuggestionRequest request,SuggestionType type)
        {
            var response = await _httpClient.GetAsync(Routes.SuggestionEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby,type));
            return await response.ToPaginatedResult<GetAllSuggestionsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditSuggestionCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SuggestionEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> SaveReplyAsync(AddEditReplyCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.SuggestionEndpoints.SaveReply, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetSuggestionByIdResponse>> GetByIdAsync(int SuggestionId)
        {
            var response = await _httpClient.GetAsync(Routes.SuggestionEndpoints.GetById(SuggestionId));
            return await response.ToResult<GetSuggestionByIdResponse>();
        }
    }
}