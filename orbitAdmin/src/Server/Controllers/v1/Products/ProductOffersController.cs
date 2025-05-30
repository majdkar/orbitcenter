using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Commands.Delete;
using SchoolV01.Application.Features.Products.Queries.GetActiveProductOffer;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Products
{
    [ControllerName("Product Offers (Services Offers)")]

    public class ProductOffersController : BaseApiController<ProductOffersController>
    {
        /// <summary>
        /// Get All Offers By Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllByProduct/{productId}")]
        public async Task<IActionResult> GetAllByProduct(int productId)
        {
            var offers = await Mediator.Send(new GetAllProductOffersQuery { ProductId = productId });
            return Ok(offers);
        }

        /// <summary>
        /// Get All Active Offers
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllActive")]
        public async Task<IActionResult> GetAllActive()
        {
            var offers = await Mediator.Send(new GetAllActiveProductOffersQuery());
            return Ok(offers);
        }

        /// <summary>
        /// Get All ProductOffers Paged 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPaged")]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productOffers = await Mediator.Send(new Application.Features.Products.Queries.GetAllPaged.GetAllPagedProductOffersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productOffers);
        }

        /// <summary>
        /// Get All ActiveProductOffers Paged 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPagedActive")]
        public async Task<IActionResult> GetAllPagedActive(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productOffers = await Mediator.Send(new GetAllPagedActiveProductOffersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productOffers);
        }

        /// <summary>
        /// Get Active Product Offer
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetActive/{productId}")]
        public async Task<IActionResult> GetActive(int productId)
        {
            var offer = await Mediator.Send(new GetActiveProductOfferQuery { ProductId = productId });
            return Ok(offer);
        }

        /// <summary>
        /// Get All PagedOffers By Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [HttpGet("GetAllPagedByProduct/{productId}")]
        public async Task<IActionResult> GetAllPagedByProduct(int productId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var offers = await Mediator.Send(new Application.Features.Products.Queries.GetAll.GetAllPagedProductOffersQuery(productId, pageNumber, pageSize, searchString, orderBy));
            return Ok(offers);
        }

        /// <summary>
        /// Get Product Offer By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetProductOfferByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a Product Offer
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductOfferCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product Offer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductOfferCommand { Id = id }));
        }
    }
}

