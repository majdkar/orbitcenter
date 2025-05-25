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
    public class GetCompanyByIdQuery : IRequest<Result<GetAllCompaniesResponse>>
    {
        public int Id { get; set; }

    }
    internal class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<GetAllCompaniesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCompanyByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCompaniesResponse>> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken)
        {
            var companyByIdFilterSpec = new CompanyByIdFilterSpecification(query.Id);
            var company = await _unitOfWork.Repository<Company>().Entities
                .Specify(companyByIdFilterSpec)
                .FirstOrDefaultAsync(cancellationToken);
            var mappedCompany = _mapper.Map<GetAllCompaniesResponse>(company);



           var client = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Id == mappedCompany.ClientId);
            if (client != null)
            {
                mappedCompany.Status = client.Status;
            }
            return await Result<GetAllCompaniesResponse>.SuccessAsync(mappedCompany);
        }
    }
}