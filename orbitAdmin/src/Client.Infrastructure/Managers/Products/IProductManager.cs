using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public interface IProductManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedAsync(GetAllPagedProductsRequest request);


        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedSearchProductAsync(GetAllPagedProductsRequest request,string productname, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice);


        Task<PaginatedResult<GetAllProductsResponse>> GetAllPagedProductByCompanyIdAsync(GetAllPagedProductsRequest request,int companyId);
        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllPagedProductByCategoryIdAsync(GetAllPagedProductsRequest request,int categoryId);


        Task<IResult<GetProductByIdResponse>> GetByIdAsync(int productId);



        Task<IResult<int>> SaveForCompanyProfileAsync(AddEditCompanyProductCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<List<GetAllProductsResponse>>> GetAllAsync();

        Task<PaginatedResult<GetAllPagedProductsResponse>> GetAllBySearchAsync(GetAllPagedProductsRequest request, string nameEn, int productParentCategoryId, int productSubCategoryId, int productSubSubCategoryId, int productSubSubSubCategoryId, int brandId, decimal retailPriceStart, decimal retailPriceEnd);

    }
}
