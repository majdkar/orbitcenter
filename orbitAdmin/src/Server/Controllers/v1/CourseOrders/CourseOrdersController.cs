using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Features.CourseOrders.Commands.Delete;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAll;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.CourseOrders.Queries.GetById;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.Export;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.CourseOrders
{
    [ControllerName("CourseOrders")]

    public class CourseOrdersController : BaseApiController<CourseOrdersController>
    {
        /// <summary>
        /// Get All CourseOrders
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var CourseOrders = await Mediator.Send(new GetAllCourseOrdersQuery());
            return Ok(CourseOrders);
        }





        /// <summary>
        /// Get All Paged CourseOrders
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseOrders = await Mediator.Send(new GetAllPagedCourseOrdersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseOrders);
        }


        /// <summary>
        /// Get All Paged Course Orders By Clinet
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedByClient")]
        public async Task<IActionResult> GetAllPagedByClient(int clientId,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseOrders = await Mediator.Send(new GetAllPagedCourseOrdersByClientQuery(pageNumber, pageSize, searchString, orderBy, clientId));
            return Ok(CourseOrders);
        }

        /// <summary>
        /// Get All Paged Search CourseOrders
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderNumber"></param>
        /// <param name="clientId"></param>
        /// <param name="courseId"></param>
        /// <param name="fromprice"></param> 
        /// <param name="toprice"></param>        
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.CourseOrders.View)]
        [AllowAnonymous]
        [HttpGet("GetAllPagedSearchCourse")]
        public async Task<IActionResult> GetAllPagedSearchCourseOrder( string orderNumber, int clientId, int courseId,  decimal fromprice, decimal toprice,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseOrders = await Mediator.Send(new GetAllPagedSearchCourseOrdersQuery(pageNumber, pageSize, searchString, orderBy, orderNumber,clientId, courseId, fromprice, toprice));
            return Ok(CourseOrders);
        }


        /// <summary>
        /// Get CourseOrder By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.CourseOrders.View)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCourseOrderByIdQuery { Id = id });
            return Ok(company);
        }

    

        /// <summary>
        /// Add/Edit a CourseOrder
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost()]
        public async Task<IActionResult> Post(AddEditCourseOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a CourseOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Orders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseOrderCommand { Id = id }));
        }

        /// <summary>
        /// Search CourseOrders and Export to Excel
        /// </summary>
     
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCourseOrdersQuery(searchString)));
        }

   

    }
}
