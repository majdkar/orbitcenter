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
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using SchoolV01.Application.Specifications.Courses;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetById
{
    public class GetCourseOrderByIdQuery : IRequest<Result<GetCourseOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseOrderByIdQueryHandler : IRequestHandler<GetCourseOrderByIdQuery, Result<GetCourseOrderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseOrderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseOrderByIdResponse>> Handle(GetCourseOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new CourseOrderByIdFilterSpecification(query.Id);
            Expression<Func<CourseOrder, GetCourseOrderByIdResponse>> expression = e => new GetCourseOrderByIdResponse
            {
                Id = e.Id,
                Price = e.Price,
                Notes = e.Notes,
                Course = e.Course,
                CourseId = e.CourseId,
                ClientId = e.ClientId,
                Status = e.Status,
                PaymentStatus = e.PaymentStatus,
                OrderNumber = e.OrderNumber,
                Client = e.Client,
                 ClientType =e.ClientType,
                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
            };

            var CourseOrder = await _unitOfWork.Repository<CourseOrder>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetCourseOrderByIdResponse>.SuccessAsync(CourseOrder);
        }
    }
}