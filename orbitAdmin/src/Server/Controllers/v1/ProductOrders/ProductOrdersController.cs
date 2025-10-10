using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Features.ProductOrders.Commands.Delete;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAll;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.ProductOrders.Queries.GetById;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.Export;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.ProductOrders
{
    [ControllerName("ProductOrders")]

    public class ProductOrdersController : BaseApiController<ProductOrdersController>
    {
        /// <summary>
        /// Get All ProductOrders
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ProductOrders = await Mediator.Send(new GetAllProductOrdersQuery());
            return Ok(ProductOrders);
        }





        /// <summary>
        /// Get All Paged ProductOrders
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
            var ProductOrders = await Mediator.Send(new GetAllPagedProductOrdersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(ProductOrders);
        }

        /// <summary>
        /// Get All Paged Search ProductOrders
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderNumber"></param>
        /// <param name="clientId"></param>
        /// <param name="ProductId"></param>
        /// <param name="fromprice"></param> 
        /// <param name="toprice"></param>        
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductOrders.View)]
        [AllowAnonymous]
        [HttpGet("GetAllPagedSearchProduct")]
        public async Task<IActionResult> GetAllPagedSearchProductOrder( string orderNumber, int clientId, int ProductId,  decimal fromprice, decimal toprice,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var ProductOrders = await Mediator.Send(new GetAllPagedSearchProductOrdersQuery(pageNumber, pageSize, searchString, orderBy, orderNumber,clientId, ProductId, fromprice, toprice));
            return Ok(ProductOrders);
        }


        /// <summary>
        /// Get ProductOrder By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductOrders.View)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetProductOrderByIdQuery { Id = id });
            return Ok(company);
        }

    

        /// <summary>
        /// Add/Edit a ProductOrder
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Orders.Create)]
        [HttpPost()]
        public async Task<IActionResult> Post(AddEditProductOrderCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Orders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductOrderCommand { Id = id }));
        }

        /// <summary>
        /// Search ProductOrders and Export to Excel
        /// </summary>
     
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportProductOrdersQuery(searchString)));
        }

   

    }
}
