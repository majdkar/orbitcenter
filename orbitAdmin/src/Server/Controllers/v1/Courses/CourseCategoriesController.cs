using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Commands.Delete;
using SchoolV01.Application.Features.CourseCategories.Queries.Export;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    [ControllerName("Course Categories")]
    public class CourseCategoriesController : BaseApiController<CourseCategoriesController>
    {
        /// <summary>
        /// Get All Paged CourseCategories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseCategories = await Mediator.Send(new GetAllPagedCourseCategoriesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseCategories);
        }
        /// <summary>
        /// Get All Paged Main CourseCategories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllMainCourseCategories")]
        public async Task<IActionResult> GetAllPagedMain(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseCategories = await Mediator.Send(new GetAllPagedMainCourseCategoriesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseCategories);
        }
        /// Get All Paged CourseCategory Sons and Classifications
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="categoryId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllSonsAndClassification")]
        public async Task<IActionResult> GetAllSonsAndClassification(int categoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseCategories = await Mediator.Send(new GetAllPagedCourseCategorySonsQuery(categoryId, pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseCategories);
        }
        /// <summary>
        /// Get Course Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCourseCategoriesByIdQuery { Id = id });
            return Ok(company);
        }
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await Mediator.Send(new GetAllCourseCategoriesQuery());
            return Ok(categories);
        }

        /// <summary>
        /// Get All Category Sons
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetCategorySons/{id}")]
        public async Task<IActionResult> GetCategorySons(int id)
        {
            var categories = await Mediator.Send(new GetAllCourseCategorySonsQuery(id));
            return Ok(categories);
        }

     


        /// <summary>
        /// Add/Edit a CourseCategory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.CourseCategories.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCourseCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a CourseCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.CourseCategories.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseCategoryCommand { Id = id }));
        }

        /// <summary>
        /// Search CourseCategories and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCourseCategoriesQuery(searchString)));
        }
    }
}
