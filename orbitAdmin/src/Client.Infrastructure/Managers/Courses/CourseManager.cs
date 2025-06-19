 
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
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

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public class CourseManager : ICourseManager
    {
        private readonly HttpClient _httpClient;
        public CourseManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedAsync(GetAllPagedCoursesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedCoursesResponse>();
        }

        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedSearchCourseAsync(GetAllPagedCoursesRequest request, string Coursename, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPagedSearchCourse(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, Coursename, propductcategoryid, propductSubcategoryid,  propductSubSubcategoryid,  propductSubSubSubcategoryid,  fromprice,  toprice));
            return await response.ToPaginatedResult<GetAllPagedCoursesResponse>();
        }



        public async Task<PaginatedResult<GetAllCoursesResponse>> GetAllPagedCourseByCompanyIdAsync(GetAllPagedCoursesRequest request,int companyId)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPagedCourseByComanyId(request.PageNumber, request.PageSize, request.SearchString, request.Orderby,companyId));
            return await response.ToPaginatedResult<GetAllCoursesResponse>();
        }
        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedCourseByCategoryIdAsync(GetAllPagedCoursesRequest request, int categoryId)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPagedCourseByCategoryId(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, categoryId));
            return await response.ToPaginatedResult<GetAllPagedCoursesResponse>();
        }

        public async Task<IResult<GetCourseByIdResponse>> GetByIdAsync(int CourseId)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetCourseById(CourseId));
            return await response.ToResult<GetCourseByIdResponse>();
        }

        public async Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyCourseCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CoursesEndpoints.SaveForCompanyProfile, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CoursesEndpoints.DeleteCourse}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync( Routes.CoursesEndpoints.ExportFilteredByCompany(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllCoursesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAll);
            return await response.ToResult<List<GetAllCoursesResponse>>();
        }

        public async Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllBySearchAsync(GetAllPagedCoursesRequest request, string nameEn, int CourseParentCategoryId, int CourseSubCategoryId, int CourseSubSubCategoryId, int CourseSubSubSubCategoryId, int brandId, decimal retailPriceStart, decimal retailPriceEnd)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllCoursesBySearch(request.PageNumber, request.PageSize, request.SearchString, request.Orderby, nameEn, CourseParentCategoryId, CourseSubCategoryId, CourseSubSubCategoryId, CourseSubSubSubCategoryId, brandId, retailPriceStart, retailPriceEnd));
            return await response.ToPaginatedResult<GetAllPagedCoursesResponse>();
        }



    }
}
