using System;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Clients;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Application.Features.Clients.Companies.Queries.GetPendingCompanies
{
    public class GetPendingCompaniesQuery:IRequest<PaginatedResult<GetAllCompaniesResponse>>
    {
        public string CompanyName { get; }
        public string Email { get; }
        public int CountryId { get; set; }
        public string PhoneNumber { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; }
        public GetPendingCompaniesQuery(int countryId, string companyName, string email, string phoneNumber, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
            CompanyName = companyName;
            Email = email;
            PhoneNumber = phoneNumber;
            CountryId= countryId;
        }

        internal class GetPendingCompaniesQueryHandler : IRequestHandler<GetPendingCompaniesQuery, PaginatedResult<GetAllCompaniesResponse>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            public GetPendingCompaniesQueryHandler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<PaginatedResult<GetAllCompaniesResponse>> Handle(GetPendingCompaniesQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<Company, GetAllCompaniesResponse>> expression = e => new GetAllCompaniesResponse
                {
                    Id = e.Id,
                    NameAr = e.NameAr,
                    NameEn = e.NameEn,
                    ClientId = e.ClientId,
                    Client = e.Client,
                    CountryId = e.CountryId,
                    Country = e.Country,
                    CityId = e.CityId,
                    City = e.City,
                    
                    Phone = e.Phone,
                    Email = e.Email,
                    Address = e.Address,
                    Website = e.Website,
                    LicenseIssuingDate = e.LicenseIssuingDate,
                    ResponsiblePersonNameAr = e.ResponsiblePersonNameAr,
                    ResponsiblePersonNameEn = e.ResponsiblePersonNameEn,
                    ResponsiblePersonMobile = e.ResponsiblePersonMobile,
                    AdditionalInfo = e.AdditionalInfo,
                    CompanyFileUrl = e.CompanyFileUrl,
                    CompanyImageUrl = e.CompanyImageUrl,
                    Status = e.Client.Status,
               
                };
                var companyFilterSpec = new PendingCompaniesFilterSpecification(request);
                if (request.OrderBy?.Any() != true)
                {
                    var data = await _unitOfWork.Repository<Company>().Entities
                       .Specify(companyFilterSpec)
                       .OrderByDescending(x => x.Id)
                       .Select(expression)
                       .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                    return data;
                }
                else
                {
                    var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                    var data = await _unitOfWork.Repository<Company>().Entities
                       .Specify(companyFilterSpec)
                       .OrderBy(ordering) // require system.linq.dynamic.core
                       .Select(expression)
                       .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                    return data;

                }
            }
        }

        }

   
}
