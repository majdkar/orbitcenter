using AutoMapper;
using LazyCache;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Application.Extensions;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Text.Json;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Application.Specifications.Courses;

namespace SchoolV01.Application.Features.CourseOrders.Queries.GetAll
{
    public class GetAllCourseOrdersQuery : IRequest<Result<List<GetAllCourseOrdersResponse>>>
    {
        public GetAllCourseOrdersQuery()
        {

        }
    }
    public class GetAllCourseOrdersCachedQueryHandler : IRequestHandler<GetAllCourseOrdersQuery, Result<List<GetAllCourseOrdersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllCourseOrdersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllCourseOrdersResponse>>> Handle(GetAllCourseOrdersQuery request, CancellationToken cancellationToken)
        {
            var filter = new CourseOrderFilterSpecification();
            Expression<Func<CourseOrder, GetAllCourseOrdersResponse>> expression =  e => new GetAllCourseOrdersResponse
            {
                Id = e.Id,
                Price = e.Price,
                Notes =e.Notes,
                Course =e.Course,
                CourseId =e.CourseId,
                ClientId =e.ClientId,
                Status =e.Status,
                PaymentStatus =e.PaymentStatus,
                OrderNumber =e.OrderNumber,
                Client =e.Client,
                OrderDate = e.OrderDate,
                ClientNameAr = e.Client.Type == "Person" ? e.Client.Person.FullName : e.Client.Company.NameAr,
                ClientNameEn = e.Client.Type == "Person" ? e.Client.Person.FullNameEn : e.Client.Company.NameEn,
            };

            var  getAllCourseOrders = await _unitOfWork.Repository<CourseOrder>().Entities
                .Specify(filter)
                .Select(expression)
                .ToListAsync();

            return await Result<List<GetAllCourseOrdersResponse>>.SuccessAsync(getAllCourseOrders);

        }

 



    }
}
