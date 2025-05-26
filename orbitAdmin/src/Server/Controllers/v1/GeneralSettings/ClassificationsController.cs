using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Application.Features.Classifications.Commands;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class ClassificationsController : BaseApiController<ClassificationsController>
    {
        /// <summary>
        /// Get All Classifications
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var Classifications = await Mediator.Send(new GetAllClassificationsQuery());
            return Ok(Classifications);
        }

        /// <summary>
        /// Get a City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Classifications.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var City = await Mediator.Send(new GetClassificationByIdQuery() { Id = id });
            return Ok(City);
        }

        /// <summary>
        /// Create/Update a City
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Classifications.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditClassificationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a City
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Classifications.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteClassificationCommand { Id = id }));
        }

        /// <summary>
        /// Search Classifications and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.Classifications.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportClassificationsQuery(searchString)));
        }

    }
}