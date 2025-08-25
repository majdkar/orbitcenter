using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Commands.Delete;
using SchoolV01.Application.Features.Products.Queries.Export;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1.Products
{
    [ControllerName("Products (Services)")]

    public class ProductsController : BaseApiController<ProductsController>
    {
        /// <summary>
        /// Get All Products
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await Mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }


        /// <summary>
        /// Get All Endpoint Products
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("allEndpoints")]
        public async Task<IActionResult> GetAllEndpointProduct()
        {
            var products = await Mediator.Send(new GetAllEndpointProductsQuery());
            return Ok(products);
        }





        /// <summary>
        /// Get All  Recent Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllRecentProducts")]
        public async Task<IActionResult> GetAllRecentProducts(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var recentproducts = await Mediator.Send(new GetAllPagedRecentProductsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(recentproducts);
        }



        /// <summary>
        /// Get All  Paged  Products By CategoryId
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="categoryId"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedProductByCategoryId")]
        public async Task<IActionResult> GetAllPagedProductByCategoryId(int categoryId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await Mediator.Send(new GetAllPagedProductsByCategoryIdQuery(pageNumber, pageSize, searchString, orderBy, categoryId));
            return Ok(products);
        }
    




        /// <summary>
        /// Get All Paged Products
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
            var products = await Mediator.Send(new GetAllPagedProductsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(products);
        }

        /// <summary>
        /// Get All Paged Search Products
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="productname"></param>
        /// <param name="propductcategoryid"></param>
        /// <param name="propductSubcategoryid"></param>
        /// <param name="propductSubSubcategoryid"></param>
        /// <param name="propductSubSubSubcategoryid"></param>
        /// <param name="fromprice"></param> 
        /// <param name="toprice"></param>        
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetAllPagedSearchProduct")]
        public async Task<IActionResult> GetAllPagedSearchProduct( string productname, int propductcategoryid,int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var products = await Mediator.Send(new GetAllPagedSearchProductsQuery(pageNumber, pageSize, searchString, orderBy, productname, propductcategoryid, propductSubcategoryid, propductSubSubcategoryid, propductSubSubSubcategoryid, fromprice, toprice));
            return Ok(products);
        }


        /// <summary>
        /// Get Product By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Products.View)]
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await Mediator.Send(new GetProductByIdQuery { Id = id });
            return Ok(company);
        }

        /// <summary>
        /// Get Product By Endpoint
        /// </summary>
        /// <param name="Endpoint"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("GetByEndpoint/{Endpoint}")]
        public async Task<IActionResult> GetByName(string Endpoint)
        {
            var company = await Mediator.Send(new GetProductByEndpointQuery { Endpoint = Endpoint });
            return Ok(company);
        }

        /// <summary>
        /// Add/Edit a Product for company profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Products.Create)]
        [HttpPost("AddEditCompanyProduct")]
        public async Task<IActionResult> Post(AddEditCompanyProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Delete a Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Products.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductCommand { Id = id }));
        }

        /// <summary>
        /// Search Products and Export to Excel
        /// </summary>
     
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await Mediator.Send(new ExportCompanyProductsQuery(searchString)));
        }

        /// <summary>
        /// Get All  Paged Products By Search
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <param name="nameEn"></param>
        /// <param name="productParentCategoryId"></param>
        /// <param name="productSubCategoryId"></param>
        /// <param name="productSubSubCategoryId"></param>
        /// <param name="productSubSubSubCategoryId"></param>
        /// <param name="brandId"></param>
        /// <param name="retailpricestart"></param>
        /// <param name="retailpriceend"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("ProductsBySearch")]
        public async Task<IActionResult> ProductsBySearch(string nameEn, int productParentCategoryId, int productSubCategoryId, int productSubSubCategoryId, int productSubSubSubCategoryId, int brandId, decimal retailpricestart, decimal retailpriceend, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var Products = await Mediator.Send(new GetAllPagedProductsBySearchQuery(pageNumber, pageSize, searchString, orderBy, nameEn, productParentCategoryId, productSubCategoryId, productSubSubCategoryId, productSubSubSubCategoryId, brandId, retailpricestart, retailpriceend));
            return Ok(Products);
        }

    }
}
