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
    public class GetProductSeoByIdQuery : IRequest<Result<GetProductSeoByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductSeoByIdQueryHandler : IRequestHandler<GetProductSeoByIdQuery, Result<GetProductSeoByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductSeoByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductSeoByIdResponse>> Handle(GetProductSeoByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new ProductSeoByIdFilterSpecification(query.Id);
            Expression<Func<ProductSeo, GetProductSeoByIdResponse>> expression = e => new GetProductSeoByIdResponse
            {
                Id = e.Id,
               
                MetaTitleAr = e.MetaTitleAr,
                MetaTitleEn = e.MetaTitleEn,
                MetaTitleGe = e.MetaTitleGe,
               
                MetaNameAr = e.MetaNameAr,
                MetaNameEn = e.MetaNameEn,
                MetaNameGe = e.MetaNameGe,
               
                MetaRobots = e.MetaRobots,
               
                ProductId = e.ProductId,
                
                MetaUrlAr = e.MetaUrlAr,
                 MetaUrlEn = e.MetaUrlEn,
                 MetaUrlGe = e.MetaUrlGe,
                
                MetaKeywordsAr = e.MetaKeywordsAr,
                 MetaKeywordsEn = e.MetaKeywordsEn,
                 MetaKeywordsGe = e.MetaKeywordsGe,

                 MetaDescriptionsAr = e.MetaDescriptionsAr,
                 MetaDescriptionsEn = e.MetaDescriptionsEn,
                 MetaDescriptionsGe = e.MetaDescriptionsGe,


                  ImageAlt1Ar = e.ImageAlt1Ar,
                  ImageAlt1En = e.ImageAlt1En,
                  ImageAlt1Ge = e.ImageAlt1Ge,

                  ImageAlt2Ar = e.ImageAlt2Ar,
                  ImageAlt2En = e.ImageAlt2En,
                  ImageAlt2Ge = e.ImageAlt2Ge,

                  ImageAlt3Ar = e.ImageAlt3Ar,
                  ImageAlt3En = e.ImageAlt3En,
                  ImageAlt3Ge = e.ImageAlt3Ge,

                  ImageAlt4Ar = e.ImageAlt4Ar,
                  ImageAlt4En = e.ImageAlt4En,
                  ImageAlt4Ge = e.ImageAlt4Ge,


                  
            };

            var ProductSeo = await _unitOfWork.Repository<ProductSeo>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetProductSeoByIdResponse>.SuccessAsync(ProductSeo);
        }
    }
}