using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.CourseTypes.Commands;
using SchoolV01.Application.Features.CourseTypes.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public interface ICourseTypeManager : IManager
    {
        Task<IResult<List<GetAllCourseTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditCourseTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}