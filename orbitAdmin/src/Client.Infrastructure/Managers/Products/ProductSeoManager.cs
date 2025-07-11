using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public class ProductSeoManager : IProductSeoManager
    {
        private readonly HttpClient _httpClient;

        public ProductSeoManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllProductSeosResponse>>> GetAllByProductAsync(int productId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllProductSeos(productId));
            return await response.ToResult<List<GetAllProductSeosResponse>>();
        }

        public async Task<PaginatedResult<GetAllProductSeosResponse>> GetAllPagedByProductAsync(GetAllPagedProductSeosRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedProductSeos(request.productId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllProductSeosResponse>();
        }

        public async Task<IResult<GetProductSeoByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetProductSeoById(id));
            return await response.ToResult<GetProductSeoByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductSeoCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.SaveSeo, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductsEndpoints.DeleteSeo}/{id}");
            return await response.ToResult<int>();
        }

 
    }
}
