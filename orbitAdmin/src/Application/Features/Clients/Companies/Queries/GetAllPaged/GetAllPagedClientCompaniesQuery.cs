using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged
{
    public  class GetAllPagedClientCompaniesQuery : IRequest<PaginatedResult<GetAllCompaniesResponse>>
    {
        
        public string CompanyName { get; }
        public int CountryId { get; set; }
        public string Email { get; }
        public string PhoneNumber { get; }
       
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }
        public GetAllPagedClientCompaniesQuery(int countryId, string companyName, string email,  string phoneNumber, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            
            CompanyName = companyName;
            Email = email;
            PhoneNumber = phoneNumber;
            
            PageNumber = pageNumber;
            PageSize = pageSize;
            CountryId = countryId;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllPagedClientCompaniesQueryHandler : IRequestHandler<GetAllPagedClientCompaniesQuery, PaginatedResult<GetAllCompaniesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedClientCompaniesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllCompaniesResponse>> Handle(GetAllPagedClientCompaniesQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Company, GetAllCompaniesResponse>> expression = e => new GetAllCompaniesResponse
            {
                Id = e.Id,
                NameAr = e.NameAr,
                Phone = e.Phone,
                Email = e.Email,
                CityName = _unitOfWork.Repository<City>().Entities.FirstOrDefault(x => x.Id == e.CityId) == null ? "" : _unitOfWork.Repository<City>().Entities.FirstOrDefault(x => x.Id == e.CityId).NameAr,
            };
            var companyFilterSpec = new CompaniesFilterSpecification(request);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Company>().Entities
                   .Specify(companyFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Company>().Entities
                   .Specify(companyFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
