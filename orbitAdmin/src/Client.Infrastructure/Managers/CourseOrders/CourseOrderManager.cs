 
using SchoolV01.Application.Features.CourseOrders.Queries.GetAll;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.CourseOrders.Queries.GetById;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.CourseOrders
{
    public class CourseOrderManager : ICourseOrderManager
    {
        private readonly HttpClient _httpClient;
        public CourseOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> GetAllPagedAsync(GetAllPagedCourseOrdersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CourseOrdersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCourseOrdersResponse>();
        }


        public async Task<IResult<GetCourseOrderByIdResponse>> GetByIdAsync(int CourseOrderId)
        {
            var response = await _httpClient.GetAsync(Routes.CourseOrdersEndpoints.GetCourseOrderById(CourseOrderId));
            return await response.ToResult<GetCourseOrderByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCourseOrderCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CourseOrdersEndpoints.SaveOrder, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CourseOrdersEndpoints.DeleteOrder}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.CourseOrdersEndpoints.ExportFilteredByCompany(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllCourseOrdersResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CourseOrdersEndpoints.GetAll);
            return await response.ToResult<List<GetAllCourseOrdersResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> GetAllPagedSearchCourseOrdersAsync(GetAllPagedCourseOrdersRequest request, string orderNumber, int clientId, int courseId, decimal fromprice, decimal toprice)
        {
            var response = await _httpClient.GetAsync(Routes.CourseOrdersEndpoints.GetAllPagedSearchCourse(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, orderNumber, clientId, courseId, fromprice, toprice));
            return await response.ToPaginatedResult<GetAllPagedCourseOrdersResponse>();
        }
    }
}
