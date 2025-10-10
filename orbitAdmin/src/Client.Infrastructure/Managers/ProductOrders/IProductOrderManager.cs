using SchoolV01.Application.Features.ProductOrders.Queries.GetAll;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.ProductOrders.Queries.GetById;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Client.Infrastructure.Managers.ProductOrders
{
    public interface IProductOrderManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductOrdersResponse>> GetAllPagedAsync(GetAllPagedProductOrdersRequest request);

        Task<PaginatedResult<GetAllPagedProductOrdersResponse>> GetAllPagedSearchProductOrdersAsync(GetAllPagedProductOrdersRequest request,string orderNumber, int clientId, int ProductId, decimal fromprice, decimal toprice);

        Task<IResult<GetProductOrderByIdResponse>> GetByIdAsync(int ProductOrdersId);

        Task<IResult<int>> SaveAsync(AddEditProductOrderCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");

        Task<IResult<List<GetAllProductOrdersResponse>>> GetAllAsync();


    }
}
