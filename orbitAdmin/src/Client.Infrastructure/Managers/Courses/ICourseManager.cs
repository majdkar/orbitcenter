using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public interface ICourseManager : IManager
    {
        Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedAsync(GetAllPagedCoursesRequest request);


        Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedSearchCourseAsync(GetAllPagedCoursesRequest request,string Coursename, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice);


        Task<PaginatedResult<GetAllCoursesResponse>> GetAllPagedCourseByCompanyIdAsync(GetAllPagedCoursesRequest request,int companyId);
        Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllPagedCourseByCategoryIdAsync(GetAllPagedCoursesRequest request,int categoryId);


        Task<IResult<GetCourseByIdResponse>> GetByIdAsync(int CourseId);



        Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyCourseCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<List<GetAllCoursesResponse>>> GetAllAsync();

        Task<PaginatedResult<GetAllPagedCoursesResponse>> GetAllBySearchAsync(GetAllPagedCoursesRequest request, string nameEn, int CourseParentCategoryId, int CourseSubCategoryId, int CourseSubSubCategoryId, int CourseSubSubSubCategoryId, int brandId, decimal retailPriceStart, decimal retailPriceEnd);

    }
}
