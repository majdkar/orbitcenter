using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Clients;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;

namespace SchoolV01.Application.Features.Clients.Companies.Queries.GetById
{
    public class GetCompanyByClientIdQuery : IRequest<Result<GetAllCompaniesResponse>>
    {
        public int ClientId { get; set; }

    }
    internal class GetCompanyByClientIdQueryHandler : IRequestHandler<GetCompanyByClientIdQuery, Result<GetAllCompaniesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCompaniesResponse>> Handle(GetCompanyByClientIdQuery query, CancellationToken cancellationToken)
        {
            var companyByIdFilterSpec = new CompanyByClientIdFilterSpecification(query.ClientId);
            var company = await _unitOfWork.Repository<Company>().Entities
                .Specify(companyByIdFilterSpec)
                .FirstOrDefaultAsync(cancellationToken);
            var mappedCompany = _mapper.Map<GetAllCompaniesResponse>(company);


            return await Result<GetAllCompaniesResponse>.SuccessAsync(mappedCompany);
        }
    }
}