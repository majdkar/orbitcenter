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

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public class ProductOfferManager : IProductOfferManager
    {
        private readonly HttpClient _httpClient;

        public ProductOfferManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllProductOffersResponse>>> GetAllByProductAsync(int productId)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllProductOffers(productId));
            return await response.ToResult<List<GetAllProductOffersResponse>>();
        }

        public async Task<PaginatedResult<GetAllProductOffersResponse>> GetAllPagedByProductAsync(GetAllPagedProductOffersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetAllPagedProductOffers(request.productId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllProductOffersResponse>();
        }

        public async Task<IResult<GetProductOfferByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoints.GetProductOfferById(id));
            return await response.ToResult<GetProductOfferByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductOfferCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductsEndpoints.SaveOffer, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductsEndpoints.DeleteOffer}/{id}");
            return await response.ToResult<int>();
        }
    }
}
