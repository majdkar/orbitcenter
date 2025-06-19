using AutoMapper;
using MediatR;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Queries.GetActiveCourseOffer
{
    public class GetActiveCourseOfferQuery : IRequest<Result<GetActiveCourseOfferResponse>>
    {
        public int CourseId { get; set; }
    }

    internal class GetActiveCourseOfferQueryHandler : IRequestHandler<GetActiveCourseOfferQuery, Result<GetActiveCourseOfferResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetActiveCourseOfferQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetActiveCourseOfferResponse>> Handle(GetActiveCourseOfferQuery request, CancellationToken cancellationToken)
        {
            var CourseOffers = _unitOfWork.Repository<CourseOffer>().Entities
                .FirstOrDefault(x => x.CourseId == request.CourseId && x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date);
            var mappedOffers = _mapper.Map<GetActiveCourseOfferResponse>(CourseOffers);
            return await Result<GetActiveCourseOfferResponse>.SuccessAsync(mappedOffers);
        }
    }
}

