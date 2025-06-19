using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAllPaged;

using SchoolV01.Application.Requests.CourseCategories;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Courses.Queries.GetById;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory
{
    public class CourseCategoryManager : ICourseCategoryManager
    {
        private readonly HttpClient _httpClient;

        public CourseCategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CourseCategoriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CourseCategoriesEndpoints.Export
                : Routes.CourseCategoriesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetCourseCategoryImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetCourseCategoryImage(id));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetCourseCategoryIconAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetCourseCategoryIcon(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetCourseCategoriesAsync(GetAllPagedCourseCategoriesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCourseCategoriesResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedCourseCategoriesRequest request,int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetAllPagedSons(categoryId,request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCourseCategoriesResponse>();
        }
        //public async Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetAllPagedCategorySonsAsync(GetAllPagedCourseCategoriesRequest request, int categoryId)
        //{
        //    var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetAllPagedSonsAndClassifications(categoryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
        //    return await response.ToPaginatedResult<GetAllPagedCourseCategoriesResponse>();
        //}

        public async Task<IResult<int>> SaveAsync(AddEditCourseCategoryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CourseCategoriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllCourseCategoriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCourseCategoriesResponse>>();
        }

        public async Task<IResult<List<GetAllCourseCategorySonsResponse>>> GetCategorySonsAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetCourseCategorySons(id));
            return await response.ToResult<List<GetAllCourseCategorySonsResponse>>();
        }
        public async Task<IResult<GetCourseCategoriesByIdResponse>> GetByIdAsync(int CourseCategoryId)
        {
            var response = await _httpClient.GetAsync(Routes.CourseCategoriesEndpoints.GetCourseCategoryById(CourseCategoryId));
            return await response.ToResult<GetCourseCategoriesByIdResponse>();
        }
    }
}
