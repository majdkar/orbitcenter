using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Suggestions.Queries.Export;
using SchoolV01.Application.Features.Suggestions.Commands.Delete;
using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;
using System.Reflection;
using SchoolV01.Domain.Enums;


namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class SuggestionsController : BaseApiController<SuggestionsController>
    {
        /// <summary>
        /// Get All Suggestions
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Suggestions.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(SuggestionType type, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var nations = await Mediator.Send(new GetAllSuggestionsQuery(pageNumber, pageSize, searchString, orderBy,type));
            return Ok(nations);
        }

        /// <summary>
        /// Get a Suggestion By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Suggestions.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Suggestion = await Mediator.Send(new GetSuggestionByIdQuery() { Id = id });
            return Ok(Suggestion);
        }


        /// <summary>
        /// Create/Update a Suggestion
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditSuggestionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Post Reply For a Suggestion
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Suggestions.Create)]
        [HttpPost("SaveReply")]
        public async Task<IActionResult> PostReply(AddEditReplyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Suggestion
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Suggestions.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteSuggestionCommand { Id = id }));
        }

        /// <summary>
        /// Search Suggestions and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //[Authorize(Policy = Permissions.Suggestions.View)]
        //[HttpGet("export")]
        //public async Task<IActionResult> Export(string searchString = "",  SuggestionType type = 0)
        //{
        //    return Ok(await Mediator.Send(new ExportSuggestionQuery(searchString,type)));
        //}
    }
}