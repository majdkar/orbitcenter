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

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public class CourseOfferManager : ICourseOfferManager
    {
        private readonly HttpClient _httpClient;

        public CourseOfferManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetAllCourseOffersResponse>>> GetAllByCourseAsync(int CourseId)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllCourseOffers(CourseId));
            return await response.ToResult<List<GetAllCourseOffersResponse>>();
        }

        public async Task<PaginatedResult<GetAllCourseOffersResponse>> GetAllPagedByCourseAsync(GetAllPagedCourseOffersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetAllPagedCourseOffers(request.CourseId, request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllCourseOffersResponse>();
        }

        public async Task<IResult<GetCourseOfferByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.CoursesEndpoints.GetCourseOfferById(id));
            return await response.ToResult<GetCourseOfferByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCourseOfferCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.CoursesEndpoints.SaveOffer, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.CoursesEndpoints.DeleteOffer}/{id}");
            return await response.ToResult<int>();
        }
    }
}
