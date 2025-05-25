using SchoolV01.Application.Features.Clients.Companies.Commands.AcceptCompanyRequest;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Companies.Commands.Delete;
using SchoolV01.Application.Features.Clients.Companies.Commands.RefuseCompanyRequest;
using SchoolV01.Application.Features.Clients.Companies.Queries.Export;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAcceptedCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetPendingCompanies;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetRefusedCompanies;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.CompaniesManagement
{
    public class CompaniesController : BaseApiController<CompaniesController>
    {

        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await Mediator.Send(new GetAllCompaniesQuery());
            return Ok(companies);
        }



        


        /// <summary>
        /// Get Paged Client Companies
        /// </summary>
       
        /// <param name="companyName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="CountryId"></param>
     
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("PagedClientCompanies")]
        public async Task<IActionResult> GetPagedClientCompanies( string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var clientCompanies = await Mediator.Send(new GetAllPagedClientCompaniesQuery(CountryId, companyName, email, phoneNumber, pageNumber, pageSize, searchString, orderBy));
            return Ok(clientCompanies);
        }


      



        /// <summary>
        /// Get Accepted Companies
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="CountryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("AcceptedPaged")]
        public async Task<IActionResult> GetAcceptedCompanies( string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var pendingcompanies = await Mediator.Send(new GetAcceptedCompaniesQuery(CountryId, companyName, email, phoneNumber, pageNumber, pageSize, searchString, orderBy));
            return Ok(pendingcompanies);
        }



        /// <summary>
        /// Get Pending Companies
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="CountryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("PendingPaged")]
        public async Task<IActionResult> GetPendingCompanies( string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var pendingcompanies = await Mediator.Send(new GetPendingCompaniesQuery(CountryId, companyName, email, phoneNumber, pageNumber, pageSize, searchString, orderBy));
            return Ok(pendingcompanies);
        }

        /// <summary>
        /// Get Refused Companies 
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="CountryId"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("RefusedPaged")]
        public async Task<IActionResult> GetRefusedCompanies( string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var refusedcompanies = await Mediator.Send(new GetRefusedCompaniesQuery(CountryId, companyName, email, phoneNumber, pageNumber, pageSize, searchString, orderBy));
            return Ok(refusedcompanies);
        }

        /// <summary>
        /// Get Company By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCompanyByIdQuery { Id = id });
            return Ok(company);
        }

        /// <summary>
        /// Get Company By ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("GetByClientId/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var company = await Mediator.Send(new GetCompanyByClientIdQuery { ClientId = clientId });
            return Ok(company);
        }

   

        /// <summary>
        /// Add/Edit a Company
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCompanyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Companies.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCompanyCommand { Id = id }));
        }

        /// <summary>
        /// Accept a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Companies.Edit)]
        [HttpDelete("accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            return Ok(await Mediator.Send(new AcceptCompanyRequestCommand { Id = id }));
        }

        /// <summary>
        /// Refuse a Company
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Companies.Edit)]
        [HttpDelete("refuse/{id}")]
        public async Task<IActionResult> Refuse(int id)
        {
            return Ok(await Mediator.Send(new RefuseCompanyRequestCommand { Id = id }));
        }

  

      

        /// <summary>
        /// Search Companies and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Companies.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCompaniesQuery(searchString)));
        }

    }
}
