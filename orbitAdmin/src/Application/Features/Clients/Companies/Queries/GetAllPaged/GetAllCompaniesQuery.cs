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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged
{
    public class GetAllCompaniesQuery : IRequest<Result<List<GetAllCompaniesResponse>>>
    {
 
        internal class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, Result<List<GetAllCompaniesResponse>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
           

            public GetAllCompaniesQueryHandler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
                
            }

            public async Task<Result<List<GetAllCompaniesResponse>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<Company, GetAllCompaniesResponse>> expression = e => new GetAllCompaniesResponse
                {
                    Id = e.Id,
                    NameAr = e.NameAr,
                    NameEn = e.NameEn,
                    ClientId = e.ClientId,
                    Client = e.Client,
                    Status = e.Client.Status,
         
                    CountryId = e.CountryId,
                    Country = e.Country,
                    CityName = e.CityName,
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
                    UserId = e.Client.UserId,
                };
                var companyFilterSpec = new CompanyFilterSpecification();
                var data = await _unitOfWork.Repository<Company>().Entities
                      .Specify(companyFilterSpec)
                      .OrderByDescending(x => x.Id)
                      .Select(expression)
                      .ToListAsync();
                return await Result<List<GetAllCompaniesResponse>>.SuccessAsync(data);

            }
        }
    }
}
