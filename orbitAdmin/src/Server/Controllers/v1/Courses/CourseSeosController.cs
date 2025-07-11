using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Commands.Delete;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Courses
{
    [ControllerName("Course Seos (Services Seos)")]

    public class CourseSeosController : BaseApiController<CourseSeosController>
    {


        /// <summary>
        /// Get All Seos By Course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("GetAllByCourse/{CourseId}")]
        public async Task<IActionResult> GetAllByCourse(int CourseId)
        {
            var Seos = await Mediator.Send(new GetAllCourseSeosQuery { CourseId = CourseId });
            return Ok(Seos);
        }


   

        /// <summary>
        /// Get Course Seo By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Courses.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCourseSeoByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a Course Seo
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Courses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCourseSeoCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Course Seo
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Courses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseSeoCommand { Id = id }));
        }
    }
}

