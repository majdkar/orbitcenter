 
using SchoolV01.Application.Features.ProductOrders.Queries.GetAll;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.ProductOrders.Queries.GetById;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
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

namespace SchoolV01.Client.Infrastructure.Managers.ProductOrders
{
    public class ProductOrderManager : IProductOrderManager
    {
        private readonly HttpClient _httpClient;
        public ProductOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedProductOrdersResponse>> GetAllPagedAsync(GetAllPagedProductOrdersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductOrdersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductOrdersResponse>();
        }


        public async Task<IResult<GetProductOrderByIdResponse>> GetByIdAsync(int ProductOrderId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductOrdersEndpoints.GetProductOrderById(ProductOrderId));
            return await response.ToResult<GetProductOrderByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductOrdersEndpoints.SaveOrder, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductOrdersEndpoints.DeleteOrder}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.ProductOrdersEndpoints.ExportFilteredByCompany(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllProductOrdersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.ProductOrdersEndpoints.GetAll);
            return await response.ToResult<List<GetAllProductOrdersResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedProductOrdersResponse>> GetAllPagedSearchProductOrdersAsync(GetAllPagedProductOrdersRequest request, string orderNumber, int clientId, int ProductId, decimal fromprice, decimal toprice)
        {
            var response = await _httpClient.GetAsync(Routes.ProductOrdersEndpoints.GetAllPagedSearchProduct(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, orderNumber, clientId, ProductId, fromprice, toprice));
            return await response.ToPaginatedResult<GetAllPagedProductOrdersResponse>();
        }
    }
}
