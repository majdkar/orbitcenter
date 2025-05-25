﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;

namespace SchoolV01.Application.Features.Clients.Persons.Queries.GetById
{
    public class GetPersonByIdQuery : IRequest<Result<GetAllPersonsResponse>>
    {
        public int Id { get; set; }
    }
    internal class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Result<GetAllPersonsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPersonByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllPersonsResponse>> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
        {
            //var personByIdFilterSpec = new PersonByIdFilterSpecification(query.Id);
            //var person = await _unitOfWork.Repository<Person>().Entities
            //    .Specify(personByIdFilterSpec)
            //    .FirstOrDefaultAsync(cancellationToken);
            var person = await _unitOfWork.Repository<Person>().Entities.FirstOrDefaultAsync(x=>x.Id==query.Id && !x.Deleted);
            if (person == null)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Not Found");
            }
            var personClient = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Id == person.ClientId && !x.Deleted);
            if (personClient == null)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Client not found");
            }
            if (!personClient.IsActive)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Client not Active");

            }
            
            var mappedPerson = _mapper.Map<GetAllPersonsResponse>(person);

       
      
            return await Result<GetAllPersonsResponse>.SuccessAsync(mappedPerson);
        }
    }
}