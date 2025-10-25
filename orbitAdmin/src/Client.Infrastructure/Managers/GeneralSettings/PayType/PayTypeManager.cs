using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;

using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.PayType
{
    public class PayTypeManager:IPayTypeManager
    {
        private readonly HttpClient _httpClient;

        public PayTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IResult<List<GetAllPayTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PayTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllPayTypesResponse>>();
        }
        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PayTypesEndpoints.Export
                : Routes.PayTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PayTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }



        public async Task<IResult<int>> SaveAsync(AddEditPayTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PayTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

       
    }
}
