using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using SchoolV01.Application.Requests.Clients.Persons;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Client.Infrastructure.Managers.Clients.Persons
{
    public interface IPersonManager:IManager
    {
        Task<IResult<List<GetAllPersonsResponse>>> GetAllAsync();
        Task<IResult<GetAllPersonsResponse>> GetByClientIdAsync(int clientId);


        Task<PaginatedResult<GetAllPersonsResponse>> GetPersonsAsync(GetAllPagedPersonsRequest request);

        Task<IResult<GetAllPersonsResponse>> GetByIdAsync(int personId);

        Task<IResult<int>> SaveAsync(AddEditPersonCommand request);


        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
