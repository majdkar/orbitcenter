using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using System.Linq.Expressions;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Text.Json;
using SchoolV01.Application.Specifications.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductsQuery : IRequest<Result<List<GetAllProductsResponse>>>
    {
        public GetAllProductsQuery()
        {

        }
    }
    public class GetAllProductsCachedQueryHandler : IRequestHandler<GetAllProductsQuery, Result<List<GetAllProductsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllProductsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllProductsResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var filter = new ProductFilterSpecification();
            Expression<Func<Product, GetAllProductsResponse>> expression =  e => new GetAllProductsResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4, 
                
                
                DescriptionEn1 = e.DescriptionAr1,
                DescriptionEn2 = e.DescriptionEn2,
                DescriptionEn3 = e.DescriptionEn3,
                DescriptionEn4 = e.DescriptionEn4,
              
                 DescriptionGe1 = e.DescriptionGe1,
                 DescriptionGe2 = e.DescriptionGe2,
                 DescriptionGe3 = e.DescriptionGe3,
                 DescriptionGe4 = e.DescriptionGe4,

          
                ProductParentCategoryId = e.ProductParentCategoryId,
                ProductSubCategoryId = e.ProductSubCategoryId,
                ProductSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductSubSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductDefaultCategoryId = e.ProductDefaultCategoryId.Value,

          
                Price = e.Price,
                Code = e.Code,
            
                Order = e.Order,
                IsVisible = e.IsVisible,
                IsRecent = e.IsRecent,
                
                NameGe = e.NameGe,
                Plan = e.Plan,
                ProductDefaultCategory = e.ProductDefaultCategory,
                ProductImageUrl1 = e.ProductImageUrl1,
                ProductImageUrl2 = e.ProductImageUrl2,
                ProductImageUrl3 = e.ProductImageUrl3,
                ProductOffers = e.ProductOffers,
            
            };

            var  getAllProducts = await _unitOfWork.Repository<Product>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllProductsResponse>>.SuccessAsync(getAllProducts);

        }

 



    }
}
