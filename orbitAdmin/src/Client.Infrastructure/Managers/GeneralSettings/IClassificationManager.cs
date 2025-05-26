using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Classifications.Commands;
using SchoolV01.Application.Features.Classifications.Queries;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings
{
    public interface IClassificationManager : IManager
    {
        Task<IResult<List<GetAllClassificationsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditClassificationCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}