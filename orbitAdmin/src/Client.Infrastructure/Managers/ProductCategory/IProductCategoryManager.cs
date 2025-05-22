using SchoolV01.Application.Features.ProductCategories.Commands.AddEdit;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAllPaged;

using SchoolV01.Application.Requests.ProductCategories;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Products.Queries.GetById;

namespace SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory
{
    public interface IProductCategoryManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetProductCategoriesAsync(GetAllPagedProductCategoriesRequest request);

        Task<IResult<List<GetAllProductCategoriesResponse>>> GetAllAsync();

        Task<IResult<List<GetAllProductCategorySonsResponse>>> GetCategorySonsAsync(int id);
        Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllCategorySonsAsync(GetAllPagedProductCategoriesRequest request,int categoryId);
        //Task<PaginatedResult<GetAllPagedProductCategoriesResponse>> GetAllPagedCategorySonsAsync(GetAllPagedProductCategoriesRequest request, int categoryId);

        Task<IResult<string>> GetProductCategoryImageAsync(int id);
        Task<IResult<string>> GetProductCategoryIconAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductCategoryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
        Task<IResult<GetProductCategoriesByIdResponse>> GetByIdAsync(int productcCtegoryId);
    }
}
