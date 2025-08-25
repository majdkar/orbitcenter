using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Application.Features.CourseTypes.Commands;
using SchoolV01.Application.Features.CourseTypes.Queries;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class CourseTypesController : BaseApiController<CourseTypesController>
    {
        /// <summary>
        /// Get All CourseTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var CourseTypes = await Mediator.Send(new GetAllCourseTypesQuery());
            return Ok(CourseTypes);
        }

        /// <summary>
        /// Get a City By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var City = await Mediator.Send(new GetCourseTypeByIdQuery() { Id = id });
            return Ok(City);
        }

        /// <summary>
        /// Create/Update a City
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.CourseTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCourseTypeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a City
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.CourseTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search CourseTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCourseTypesQuery(searchString)));
        }

    }
}