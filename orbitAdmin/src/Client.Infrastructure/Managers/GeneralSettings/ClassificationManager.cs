using SchoolV01.Application.Features.Classifications.Commands;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public class ClassificationManager : IClassificationManager
    {
        private readonly HttpClient _httpClient;

        public ClassificationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.ClassificationsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ClassificationsEndpoints.Endpoints}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllClassificationsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ClassificationsEndpoints.GetAll);
            return await response.ToResult<List<GetAllClassificationsResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClassificationCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ClassificationsEndpoints.Endpoints, request);
            return await response.ToResult<int>();
        }
    }
}