using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.PayType
{
    public interface IPayTypeManager :IManager
    {
        Task<IResult<List<GetAllPayTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditPayTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
