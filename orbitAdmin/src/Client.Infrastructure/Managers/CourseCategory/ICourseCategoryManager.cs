using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAllPaged;

using SchoolV01.Application.Requests.CourseCategories;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Courses.Queries.GetById;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory
{
    public interface ICourseCategoryManager : IManager
    {
        Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetCourseCategoriesAsync(GetAllPagedCourseCategoriesRequest request);

        Task<IResult<List<GetAllCourseCategoriesResponse>>> GetAllAsync();

        Task<IResult<List<GetAllCourseCategorySonsResponse>>> GetCategorySonsAsync(int id);
        Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedCourseCategoriesRequest request,int categoryId);
        //Task<PaginatedResult<GetAllPagedCourseCategoriesResponse>> GetAllPagedCategorySonsAsync(GetAllPagedCourseCategoriesRequest request, int categoryId);

        Task<IResult<string>> GetCourseCategoryImageAsync(int id);
        Task<IResult<string>> GetCourseCategoryIconAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditCourseCategoryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<GetCourseCategoriesByIdResponse>> GetByIdAsync(int CoursecCtegoryId);
    }
}
