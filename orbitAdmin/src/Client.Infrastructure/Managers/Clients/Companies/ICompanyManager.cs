using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;
using SchoolV01.Application.Requests.Clients.Companies;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Clients.Companies
{
    public interface ICompanyManager : IManager
    {
        Task<IResult<List<GetAllCompaniesResponse>>> GetAllAcceptedAsync();

        Task<PaginatedResult<GetAllCompaniesResponse>> GetPendingAsync(GetAllPagedCompaniesByTypeRequest request);
        Task<PaginatedResult<GetAllCompaniesResponse>> GetAcceptedAsync(GetAllPagedCompaniesByTypeRequest request);
        Task<PaginatedResult<GetAllCompaniesResponse>> GetRefusedAsync(GetAllPagedCompaniesByTypeRequest request);
        Task<PaginatedResult<GetAllCompaniesResponse>> GetAllPagedClientCompaniesAsync(GetAllPagedCompaniesByTypeRequest request);
        
        Task<IResult<GetAllCompaniesResponse>> GetByIdAsync(int companyId);
        Task<IResult<GetAllCompaniesResponse>> GetByClientIdAsync(int clientId);

     
        Task<IResult<int>> SaveAsync(AddEditCompanyCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<int>> AcceptAsync(int id);

        Task<IResult<int>> RefuseAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}
