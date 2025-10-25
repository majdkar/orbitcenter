using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Features.PayTypes.Commands.Delete;
using SchoolV01.Application.Features.PayTypes.Queries.Export;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;
using SchoolV01.Application.Features.PayTypes.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class PayTypesController : BaseApiController<PayTypesController>
    {
        /// <summary>
        /// Get All PayTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PayTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var PayTypes = await Mediator.Send(new GetAllPayTypesQuery());
            return Ok(PayTypes);
        }

        /// <summary>
        /// Get a PayType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.PayTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var PayType = await Mediator.Send(new GetPayTypeByIdQuery() { Id = id });
            return Ok(PayType);
        }

        /// <summary>
        /// Create/Update a PayType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PayTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPayTypeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a PayType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PayTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeletePayTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search PayTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.PayTypes.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportPayTypesQuery(searchString)));
        }
    }
}
