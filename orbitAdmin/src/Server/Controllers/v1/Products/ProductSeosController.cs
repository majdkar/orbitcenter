using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Commands.Delete;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Products
{
    [ControllerName("Product Seos (Services Seos)")]

    public class ProductSeosController : BaseApiController<ProductSeosController>
    {


        /// <summary>
        /// Get All Seos By Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllByProduct/{productId}")]
        public async Task<IActionResult> GetAllByProduct(int productId)
        {
            var Seos = await Mediator.Send(new GetAllProductSeosQuery { ProductId = productId });
            return Ok(Seos);
        }


        /// <summary>
        /// Get All PagedSeos By Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedByProduct/{productId}")]
        public async Task<IActionResult> GetAllPagedByProduct(int productId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Seos = await Mediator.Send(new GetAllPagedProductSeosQuery(productId, pageNumber, pageSize, searchString, orderBy));
            return Ok(Seos);
        }

        /// <summary>
        /// Get Product Seo By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetProductSeoByIdQuery { Id = id });
            return Ok(company);
        }


        /// <summary>
        /// Add/Edit a Product Seo
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductSeoCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product Seo
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductSeoCommand { Id = id }));
        }
    }
}

