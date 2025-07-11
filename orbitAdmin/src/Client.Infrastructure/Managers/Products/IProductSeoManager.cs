using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SchoolV01.Application.Requests.Products;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public interface IProductSeoManager : IManager
    {
        Task<IResult<List<GetAllProductSeosResponse>>> GetAllByProductAsync(int productId);

        Task<PaginatedResult<GetAllProductSeosResponse>> GetAllPagedByProductAsync(GetAllPagedProductSeosRequest request);
        Task<IResult<GetProductSeoByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductSeoCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
