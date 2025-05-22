using SchoolV01.Application.Features.ProductCategories.Commands.AddEdit;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAllPaged;

using SchoolV01.Application.Requests.ProductCategories;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Products.Queries.GetById;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory
{
    public class ProductCategoryManager : IProductCategoryManager
    {
        private readonly HttpClient _httpClient;

        public ProductCategoryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductCategoriesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ProductCategoriesEndpoints.Export
                : Routes.ProductCategoriesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetProductCategoryImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetProductCategoryImage(id));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetProductCategoryIconAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetProductCategoryIcon(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetProductCategoriesAsync(GetAllPagedProductCategoriesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductCategoriesResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedProductCategoriesRequest request,int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllPagedSons(categoryId,request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductCategoriesResponse>();
        }
        //public async Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllPagedCategorySonsAsync(GetAllPagedProductCategoriesRequest request, int categoryId)
        //{
        //    var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAllPagedSonsAndClassifications(categoryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
        //    return await response.ToPaginatedResult<GetAllPagedProductCategoriesResponse>();
        //}

        public async Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductCategoriesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductCategoriesResponse>>();
        }

        public async Task<IResult<List<GetAllProductCategorySonsResponse>>> GetCategorySonsAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetProductCategorySons(id));
            return await response.ToResult<List<GetAllProductCategorySonsResponse>>();
        }
        public async Task<IResult<GetProductCategoriesByIdResponse>> GetByIdAsync(int productCategoryId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductCategoriesEndpoints.GetProductCategoryById(productCategoryId));
            return await response.ToResult<GetProductCategoriesByIdResponse>();
        }
    }
}
