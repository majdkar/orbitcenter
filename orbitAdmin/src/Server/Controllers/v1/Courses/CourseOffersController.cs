using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Commands.Delete;
using SchoolV01.Application.Features.Courses.Queries.GetActiveCourseOffer;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Courses
{
    [ControllerName("Course Offers")]

    public class CourseOffersController : BaseApiController<CourseOffersController>
    {
        /// <summary>
        /// Get All Offers By Course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllByCourse/{CourseId}")]
        public async Task<IActionResult> GetAllByCourse(int CourseId)
        {
            var offers = await Mediator.Send(new GetAllCourseOffersQuery { CourseId = CourseId });
            return Ok(offers);
        }

        /// <summary>
        /// Get All Active Offers
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var offers = await Mediator.Send(new GetAllActiveCourseOffersQuery());
            return Ok(offers);
        }

        /// <summary>
        /// Get All CourseOffers Paged 
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
            var CourseOffers = await Mediator.Send(new Application.Features.Courses.Queries.GetAllPaged.GetAllPagedCourseOffersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseOffers);
        }

        /// <summary>
        /// Get All ActiveCourseOffers Paged 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedActive")]
        public async Task<IActionResult> GetAllPagedActive(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var CourseOffers = await Mediator.Send(new GetAllPagedActiveCourseOffersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(CourseOffers);
        }

        /// <summary>
        /// Get Active Course Offer
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetActive/{CourseId}")]
        public async Task<IActionResult> GetActive(int CourseId)
        {
            var offer = await Mediator.Send(new GetActiveCourseOfferQuery { CourseId = CourseId });
            return Ok(offer);
        }

        /// <summary>
        /// Get All PagedOffers By Course
        /// </summary>
        /// <param name="CourseId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedByCourse/{CourseId}")]
        public async Task<IActionResult> GetAllPagedByCourse(int CourseId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var offers = await Mediator.Send(new Application.Features.Courses.Queries.GetAll.GetAllPagedCourseOffersQuery(CourseId, pageNumber, pageSize, searchString, orderBy));
            return Ok(offers);
        }

        /// <summary>
        /// Get Course Offer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetCourseOfferByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a Course Offer
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Courses.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditCourseOfferCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Course Offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Courses.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCourseOfferCommand { Id = id }));
        }
    }
}

