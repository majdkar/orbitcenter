using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using ClientNameSpace = SchoolV01.Domain.Entities.Clients;
using SchoolV01.Application.Specifications.Clients;
using SchoolV01.Shared.Wrapper;
using System.Linq;
using SchoolV01.Application.Extensions;
using System.Linq.Dynamic.Core;
using SchoolV01.Domain.Contracts;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;

namespace SchoolV01.Application.Features.Clients.Persons.Queries.GetAllPaged
{
    public class GetAllPagedPersonsQuery : IRequest<PaginatedResult<GetAllPersonsResponse>>
    {
        public string PersonName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedPersonsQuery(string personName, string email, string phoneNumber, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PersonName = personName;
            Email = email;
            PhoneNumber = phoneNumber;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }
    internal class GetAllPagedPersonsQueryHandler : IRequestHandler<GetAllPagedPersonsQuery,PaginatedResult<GetAllPersonsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedPersonsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPersonsResponse>> Handle(GetAllPagedPersonsQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Person, GetAllPersonsResponse>> expression = e => new GetAllPersonsResponse
            {
                Id = e.Id,
                ClientId = e.ClientId,
                FullNameAr = e.FullNameAr,
                BirthDate = e.BirthDate,
                Email = e.Email,
                Phone = e.Phone,
                UserId = e.Client.UserId,
                AdditionalInfo = e.AdditionalInfo,
                Address = e.Address,
                Country = e.Country,
                CountryId = e.CountryId,
                CvFileUrl = e.CvFileUrl,
                Fax = e.Fax,
                FullNameEn = e.FullNameEn,
                IdentifierImageUrl = e.IdentifierImageUrl,
                MailBox = e.MailBox,
                PersomImageUrl = e.PersomImageUrl,
                CityId = e.CityId,
                City = e.City,
                Sex = e.Sex,

            };
            var ownerFilterSpec = new SearchPersonFilterSpecification(request);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(ownerFilterSpec)
                   .OrderByDescending(x => x.Id)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(ownerFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}