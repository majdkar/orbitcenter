using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Products;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.Products.Queries.GetById
{
    public class GetProductByIdQuery : IRequest<Result<GetProductByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<GetProductByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductByIdResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new ProductByIdFilterSpecification(query.Id);
            Expression<Func<Product, GetProductByIdResponse>> expression = e => new GetProductByIdResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                NameEn = e.NameEn,
                DescriptionAr1 = e.DescriptionAr1,
                DescriptionAr2 = e.DescriptionAr2,
                DescriptionAr3 = e.DescriptionAr3,
                DescriptionAr4 = e.DescriptionAr4,


                DescriptionEn1 = e.DescriptionEn1,
                DescriptionEn2 = e.DescriptionEn2,
                DescriptionEn3 = e.DescriptionEn3,
                DescriptionEn4 = e.DescriptionEn4,

                DescriptionGe1 = e.DescriptionGe1,
                DescriptionGe2 = e.DescriptionGe2,
                DescriptionGe3 = e.DescriptionGe3,
                DescriptionGe4 = e.DescriptionGe4,


                ProductParentCategoryId = e.ProductParentCategoryId,
                ProductParentCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameAr,
                ProductParentCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameEn,
                ProductParentCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductParentCategoryId).NameGe,


                ProductSubCategoryId = e.ProductSubCategoryId,
                ProductSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameAr,
                ProductSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameEn,
                ProductSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubCategoryId).NameGe,



                ProductSubSubCategoryId = e.ProductSubSubCategoryId,
                ProductSubSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameAr,
                ProductSubSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameEn,
                ProductSubSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubCategoryId).NameGe,



                ProductSubSubSubCategoryId = e.ProductSubSubSubCategoryId,
                ProductSubSubSubCategoryNameAr = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameAr,
                ProductSubSubSubCategoryNameEn = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameEn,
                ProductSubSubSubCategoryNameGe = _unitOfWork.Repository<ProductCategory>().Entities.FirstOrDefault(x => x.Id == e.ProductSubSubSubCategoryId).NameGe,

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
                 
                 Keywords = e.Keywords,
                  SeoDescription = e.SeoDescription,

            };

            var product = await _unitOfWork.Repository<Product>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetProductByIdResponse>.SuccessAsync(product);
        }
    }
}