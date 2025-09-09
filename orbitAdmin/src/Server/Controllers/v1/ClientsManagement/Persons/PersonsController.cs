using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Features.Clients.Companies.Commands.AcceptCompanyRequest;
using SchoolV01.Application.Features.Clients.Companies.Commands.AcceptPersonRequest;
using SchoolV01.Application.Features.Clients.Companies.Commands.RefuseCompanyRequest;
using SchoolV01.Application.Features.Clients.Companies.Commands.RefusePersonRequest;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Persons.Commands.Delete;
using SchoolV01.Application.Features.Clients.Persons.Queries.Export;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.PersonsManagement
{
    public class PersonsController : BaseApiController<PersonsController>
    {

        /// <summary>
        /// Get All Persons Paged Result
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="Status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.View)]
        [HttpGet]
        public async Task<IActionResult> GetAllPaged(string personName, string email, string phoneNumber,string Status, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var persons = await Mediator.Send(new GetAllPagedPersonsQuery(personName, email, phoneNumber,Status, pageNumber, pageSize, searchString, orderBy));
            return Ok(persons);
        }
        /// <summary>
        /// Get Person By ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.View)]
        [HttpGet("GetByClientId/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var person = await Mediator.Send(new GetPersonByClientIdQuery { ClientId = clientId });
            return Ok(person);
        }

        /// <summary>
        /// Get All Persons
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.View)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var persons = await Mediator.Send(new GetAllPersonsQuery());
            return Ok(persons);
        }

        /// <summary>
        /// Get Person By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var person = await Mediator.Send(new GetPersonByIdQuery { Id = id });
            return Ok(person);
        }


        /// <summary>
        /// Add/Edit a Person
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Person.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeletePersonCommand { Id = id }));
        }

   
        /// <summary>
        /// Search Persons and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Person.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportPersonsQuery(searchString)));
        }




        /// <summary>
        /// Accept a Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Person.Edit)]
        [HttpDelete("accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            return Ok(await Mediator.Send(new AcceptPersonRequestCommand { Id = id }));
        }

        /// <summary>
        /// Refuse a Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Person.Edit)]
        [HttpDelete("refuse/{id}")]
        public async Task<IActionResult> Refuse(int id)
        {
            return Ok(await Mediator.Send(new RefusePersonRequestCommand { Id = id }));
        }

    }
}
