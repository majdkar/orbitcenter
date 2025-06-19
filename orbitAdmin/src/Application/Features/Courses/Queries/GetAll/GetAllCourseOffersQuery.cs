using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Courses.Queries.GetAll
{
    public class GetAllCourseOffersQuery : IRequest<Result<List<GetAllCourseOffersResponse>>>
    {
        public int CourseId { get; set; }
    }

    internal class GetAllCourseOffersQueryHandler : IRequestHandler<GetAllCourseOffersQuery, Result<List<GetAllCourseOffersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCourseOffersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCourseOffersResponse>>> Handle(GetAllCourseOffersQuery request, CancellationToken cancellationToken)
        {

            var CourseOffers = _unitOfWork.Repository<CourseOffer>().Entities.Where(x => x.CourseId == request.CourseId);
            var mappedOffers = _mapper.Map<List<GetAllCourseOffersResponse>>(CourseOffers);
         

            return await Result<List<GetAllCourseOffersResponse>>.SuccessAsync(mappedOffers);
        }
    }
}