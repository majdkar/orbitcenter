using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.ProductCategories.Commands.AddEdit;
using SchoolV01.Application.Features.ProductCategories.Commands.Delete;
using SchoolV01.Application.Features.ProductCategories.Queries.Export;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class ProductCategoriesController : BaseApiController<ProductCategoriesController>
    {
        /// <summary>
        /// Get All Paged ProductCategories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet]
        public async Task<IActionResult> GetAllPaged(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productCategories = await Mediator.Send(new GetAllPagedProductCategoriesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productCategories);
        }
        /// <summary>
        /// Get All Paged Main ProductCategories
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllMainProductCategories")]
        public async Task<IActionResult> GetAllPagedMain(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productCategories = await Mediator.Send(new GetAllPagedMainProductCategoriesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productCategories);
        }
        /// Get All Paged ProductCategory Sons and Classifications
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="categoryId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAllSonsAndClassification")]
        public async Task<IActionResult> GetAllSonsAndClassification(int categoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productCategories = await Mediator.Send(new GetAllPagedProductCategorySonsQuery(categoryId, pageNumber, pageSize, searchString, orderBy));
            return Ok(productCategories);
        }
        /// <summary>
        /// Get Product Category By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetProductCategoriesByIdQuery { Id = id });
            return Ok(company);
        }
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await Mediator.Send(new GetAllProductCategoriesQuery());
            return Ok(categories);
        }

        /// <summary>
        /// Get All Category Sons
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("GetCategorySons/{id}")]
        public async Task<IActionResult> GetCategorySons(int id)
        {
            var categories = await Mediator.Send(new GetAllProductCategorySonsQuery(id));
            return Ok(categories);
        }

     


        /// <summary>
        /// Add/Edit a ProductCategory
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductCategory
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.ProductCategories.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductCategoryCommand { Id = id }));
        }

        /// <summary>
        /// Search ProductCategories and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductCategories.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportProductCategoriesQuery(searchString)));
        }
    }
}
