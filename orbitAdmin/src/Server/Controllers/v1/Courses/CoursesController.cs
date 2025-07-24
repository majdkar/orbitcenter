using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Commands.Delete;
using SchoolV01.Application.Features.Courses.Queries.Export;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Courses
{
    [ControllerName("Courses")]

    public class CoursesController : BaseApiController<CoursesController>
    {
        /// <summary>
        /// Get All Courses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Courses = await Mediator.Send(new GetAllCoursesQuery());
            return Ok(Courses);
        }




        /// <summary>
        /// Get All Endpoint Courses
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("allEndpoints")]
        public async Task<IActionResult> GetAllEndpointCourse()
        {
            var Courses = await Mediator.Send(new GetAllEndpointCoursesQuery());
            return Ok(Courses);
        }


        /// <summary>
        /// Get All  Recent Courses
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("GetAllRecentCourses")]
        public async Task<IActionResult> GetAllRecentCourses(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var recentCourses = await Mediator.Send(new GetAllPagedRecentCoursesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(recentCourses);
        }


      
        /// <summary>
        /// Get All  Paged  Courses By CategoryId
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="categoryId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("GetAllPagedCourseByCategoryId")]
        public async Task<IActionResult> GetAllPagedCourseByCategoryId(int categoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Courses = await Mediator.Send(new GetAllPagedCoursesByCategoryIdQuery(pageNumber, pageSize, searchString, orderBy, categoryId));
            return Ok(Courses);
        }
    




        /// <summary>
        /// Get All Paged Courses
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Courses = await Mediator.Send(new GetAllPagedCoursesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(Courses);
        }

        /// <summary>
        /// Get All Paged Search Courses
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="Coursename"></param>
        /// <param name="propductcategoryid"></param>
        /// <param name="propductSubcategoryid"></param>
        /// <param name="propductSubSubcategoryid"></param>
        /// <param name="propductSubSubSubcategoryid"></param>
        /// <param name="fromprice"></param> 
        /// <param name="toprice"></param>        
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [AllowAnonymous]
        [HttpGet("GetAllPagedSearchCourse")]
        public async Task<IActionResult> GetAllPagedSearchCourse( string Coursename, int propductcategoryid,int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Courses = await Mediator.Send(new GetAllPagedSearchCoursesQuery(pageNumber, pageSize, searchString, orderBy, Coursename, propductcategoryid, propductSubcategoryid, propductSubSubcategoryid, propductSubSubSubcategoryid, fromprice, toprice));
            return Ok(Courses);
        }


        /// <summary>
        /// Get Course By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCourseByIdQuery { Id = id });
            return Ok(company);
        }

        /// <summary>
        /// Get Course By Endpoint
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("GetByEndpoint/{Endpoint}")]
        public async Task<IActionResult> GetByName(string Endpoint)
        {
            var company = await Mediator.Send(new GetCourseByEndpointQuery { Endpoint = Endpoint });
            return Ok(company);
        }

        /// <summary>
        /// Add/Edit a Course for company profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Courses.Create)]
        [HttpPost("AddEditCompanyCourse")]
        public async Task<IActionResult> Post(AddEditCompanyCourseCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Course
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Courses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseCommand { Id = id }));
        }

        /// <summary>
        /// Search Courses and Export to Excel
        /// </summary>
     
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCompanyCoursesQuery(searchString)));
        }

        /// <summary>
        /// Get All  Paged Courses By Search
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="nameEn"></param>
        /// <param name="CourseParentCategoryId"></param>
        /// <param name="CourseSubCategoryId"></param>
        /// <param name="CourseSubSubCategoryId"></param>
        /// <param name="CourseSubSubSubCategoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="retailpricestart"></param>
        /// <param name="retailpriceend"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("CoursesBySearch")]
        public async Task<IActionResult> CoursesBySearch(string nameEn, int CourseParentCategoryId, int CourseSubCategoryId, int CourseSubSubCategoryId, int CourseSubSubSubCategoryId, int brandId, decimal retailpricestart, decimal retailpriceend, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Courses = await Mediator.Send(new GetAllPagedCoursesBySearchQuery(pageNumber, pageSize, searchString, orderBy, nameEn, CourseParentCategoryId, CourseSubCategoryId, CourseSubSubCategoryId, CourseSubSubSubCategoryId, brandId, retailpricestart, retailpriceend));
            return Ok(Courses);
        }

    }
}
