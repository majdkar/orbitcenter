using SchoolV01.Application.Features.CourseTypes.Commands;
using SchoolV01.Application.Features.CourseTypes.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public class CourseTypeManager : ICourseTypeManager
    {
        private readonly HttpClient _httpClient;

        public CourseTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.CourseTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CourseTypesEndpoints.Endpoints}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCourseTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CourseTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCourseTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCourseTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CourseTypesEndpoints.Endpoints, request);
            return await response.ToResult<int>();
        }
    }
}