﻿using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Features.DocumentTypes.Commands;
using SchoolV01.Application.Features.DocumentTypes.Queries;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Client.Infrastructure.Managers.Misc.DocumentType
{
    public class DocumentTypeManager : IDocumentTypeManager
    {
        private readonly HttpClient _httpClient;

        public DocumentTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DocumentTypesEndpoints.Export
                : Routes.DocumentTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DocumentTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDocumentTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DocumentTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllDocumentTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DocumentTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}