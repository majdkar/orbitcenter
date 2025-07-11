using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Client.Infrastructure.Extensions;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public class CourseSeoManager : ICourseSeoManager
    {
        private readonly HttpClient _httpClient;

        public CourseSeoManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllCourseSeosResponse>>> GetAllByCourseAsync(int CourseId)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllCourseSeos(CourseId));
            return await response.ToResult<List<GetAllCourseSeosResponse>>();
        }

        public async Task<PaginatedResult<GetAllCourseSeosResponse>> GetAllPagedByCourseAsync(GetAllPagedCourseSeosRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPagedCourseSeos(request.CourseId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCourseSeosResponse>();
        }

        public async Task<IResult<GetCourseSeoByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetCourseSeoById(id));
            return await response.ToResult<GetCourseSeoByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCourseSeoCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CoursesEndpoints.SaveSeo, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CoursesEndpoints.DeleteSeo}/{id}");
            return await response.ToResult<int>();
        }

 
    }
}
