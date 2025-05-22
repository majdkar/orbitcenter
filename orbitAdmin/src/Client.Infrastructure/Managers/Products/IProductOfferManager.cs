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

namespace SchoolV01.Client.Infrastructure.Managers.Products
{
    public interface IProductOfferManager : IManager
    {
        Task<IResult<List<GetAllProductOffersResponse>>> GetAllByProductAsync(int productId);
        Task<PaginatedResult<GetAllProductOffersResponse>> GetAllPagedByProductAsync(GetAllPagedProductOffersRequest request);
        Task<IResult<GetProductOfferByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductOfferCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
