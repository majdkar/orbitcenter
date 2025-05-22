using AutoMapper;
using MediatR;
using SchoolV01.Application.Features.Products.Queries.GetById;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Core.Entities;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;

namespace SchoolV01.Application.Features.Products.Queries.GetById
{
    public class GetProductCategoriesByIdQuery : IRequest<Result<GetProductCategoriesByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductCategoriesByIdQueryHandler : IRequestHandler<GetProductCategoriesByIdQuery, Result<GetProductCategoriesByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductCategoriesByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductCategoriesByIdResponse>> Handle(GetProductCategoriesByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<ProductCategory>().GetByIdAsync(query.Id);
            var mappedproduct = _mapper.Map<GetProductCategoriesByIdResponse>(product);
            return await Result<GetProductCategoriesByIdResponse>.SuccessAsync(mappedproduct);
        }
    }
}