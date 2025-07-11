using MediatR;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Catalog;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Linq.Dynamic.Core;

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Specifications.Products;

namespace SchoolV01.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductSeosQuery : IRequest<PaginatedResult<GetAllPagedProductSeosResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public string[] OrderBy { get; set; }
        public int ProductId { get; set; }
        public GetAllPagedProductSeosQuery(int productId,int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            ProductId = productId;
        }
    }

    internal class GetAllPagedProductSeosQueryHandler : IRequestHandler<GetAllPagedProductSeosQuery, PaginatedResult<GetAllPagedProductSeosResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedProductSeosQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedProductSeosResponse>> Handle(GetAllPagedProductSeosQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ProductSeo, GetAllPagedProductSeosResponse>> expression = e => new GetAllPagedProductSeosResponse
            {
                Id = e.Id,
                 Product = e.Product,
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

            var productOfferFilterSpec = new ProdectSeoSpecification(request.ProductId, request.SearchString);

            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<ProductSeo>().Entities.Specify(productOfferFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<ProductSeo>().Entities
                   .Specify(productOfferFilterSpec).OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
