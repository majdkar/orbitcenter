using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Requests.Clients.Companies;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Clients.Companies
{
    public class CompanyManager : ICompanyManager
    {
        private readonly HttpClient _httpClient;

        public CompanyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllCompaniesResponse>> GetPendingAsync(GetAllPagedCompaniesByTypeRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllPendingPaged(request.CompanyName, request.Email, request.PhoneNumber,request.CountryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCompaniesResponse>();
        }

        public async Task<PaginatedResult<GetAllCompaniesResponse>> GetAcceptedAsync(GetAllPagedCompaniesByTypeRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllAcceptedPaged(request.CompanyName, request.Email, request.PhoneNumber, request.CountryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCompaniesResponse>();
        }


        public async Task<PaginatedResult<GetAllCompaniesResponse>> GetAllPagedClientCompaniesAsync(GetAllPagedCompaniesByTypeRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllPagedClientCompanies(request.CompanyName, request.Email, request.PhoneNumber,  request.CountryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCompaniesResponse>();
        }


        public async Task<PaginatedResult<GetAllCompaniesResponse>> GetRefusedAsync(GetAllPagedCompaniesByTypeRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllRefusedPaged(request.CompanyName, request.Email, request.PhoneNumber, request.CountryId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCompaniesResponse>();
        }

        public async Task<IResult<List<GetAllCompaniesResponse>>> GetAllAcceptedAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetAllAccepted());
            return await response.ToResult<List<GetAllCompaniesResponse>>();
        }

        public async Task<IResult<GetAllCompaniesResponse>> GetByIdAsync(int companyId)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetById(companyId));
            return await response.ToResult<GetAllCompaniesResponse>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompaniesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> AcceptAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompaniesEndpoints.Accept}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> RefuseAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CompaniesEndpoints.Refuse}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.CompaniesEndpoints.Export
                : Routes.CompaniesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditCompanyCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CompaniesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetAllCompaniesResponse>> GetByClientIdAsync(int clientId)
        {
            var response = await _httpClient.GetAsync(Routes.CompaniesEndpoints.GetByClientId(clientId));
            return await response.ToResult<GetAllCompaniesResponse>();
        }

     
    }
}
