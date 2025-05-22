 
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;
        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedAsync(GetAllPagedProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedSearchProductAsync(GetAllPagedProductsRequest request, string productname, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedSearchProduct(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, productname, propductcategoryid, propductSubcategoryid,  propductSubSubcategoryid,  propductSubSubSubcategoryid,  fromprice,  toprice));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }



        public async Task<PaginatedResult<GetAllProductsResponse>> GetAllPagedProductByCompanyIdAsync(GetAllPagedProductsRequest request,int companyId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedProductByComanyId(request.PageNumber, request.PageSize, request.SearchString, request.Orderby,companyId));
            return await response.ToPaginatedResult<GetAllProductsResponse>();
        }
        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedProductByCategoryIdAsync(GetAllPagedProductsRequest request, int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedProductByCategoryId(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, categoryId));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }

        public async Task<IResult<GetProductByIdResponse>> GetByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetProductById(productId));
            return await response.ToResult<GetProductByIdResponse>();
        }

        public async Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.SaveForCompanyProfile, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductsEndpoints.DeleteProduct}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.ProductsEndpoints.ExportFilteredByCompany(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllProductsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductsResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllBySearchAsync(GetAllPagedProductsRequest request, string nameEn, int productParentCategoryId, int productSubCategoryId, int productSubSubCategoryId, int productSubSubSubCategoryId, int brandId, decimal retailPriceStart, decimal retailPriceEnd)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllProductsBySearch(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, nameEn, productParentCategoryId, productSubCategoryId, productSubSubCategoryId, productSubSubSubCategoryId, brandId, retailPriceStart, retailPriceEnd));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }



    }
}
