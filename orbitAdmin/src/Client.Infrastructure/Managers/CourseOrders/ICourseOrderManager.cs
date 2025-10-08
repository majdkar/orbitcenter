using SchoolV01.Application.Features.CourseOrders.Queries.GetAll;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.CourseOrders.Queries.GetById;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.CourseOrders
{
    public interface ICourseOrderManager : IManager
    {
        Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> GetAllPagedAsync(GetAllPagedCourseOrdersRequest request);

        Task<PaginatedResult<GetAllPagedCourseOrdersResponse>> GetAllPagedSearchCourseOrdersAsync(GetAllPagedCourseOrdersRequest request,string orderNumber, int clientId, int courseId, decimal fromprice, decimal toprice);

        Task<IResult<GetCourseOrderByIdResponse>> GetByIdAsync(int CourseOrdersId);

        Task<IResult<int>> SaveAsync(AddEditCourseOrderCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<List<GetAllCourseOrdersResponse>>> GetAllAsync();


    }
}
