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
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.Courses.Queries.GetById
{
    public class GetCourseOfferByIdQuery : IRequest<Result<GetCourseOfferByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseOfferByIdQueryHandler : IRequestHandler<GetCourseOfferByIdQuery, Result<GetCourseOfferByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseOfferByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseOfferByIdResponse>> Handle(GetCourseOfferByIdQuery query, CancellationToken cancellationToken)
        {
            var property = await _unitOfWork.Repository<CourseOffer>().GetByIdAsync(query.Id);
            var mappedproperty = _mapper.Map<GetCourseOfferByIdResponse>(property);
            //var Course = _unitOfWork.Repository<Course>().GetByIdAsync(mappedproperty.CourseId).Result;
            //mappedproperty.Price = Course.Price;
            //mappedproperty.Weight = Course.Weight;
            //if (Course.UnitId.HasValue && Course.UnitId.Value > 0)
            //    mappedproperty.Unit = _unitOfWork.Repository<SchoolV01.Domain.Entities.GeneralSettings.Unit>().GetByIdAsync(Course.UnitId.Value).Result;
            //if (Course.CurrencyId.HasValue && Course.CurrencyId.Value > 0)
            //    mappedproperty.Currency = _unitOfWork.Repository<Currency>().GetByIdAsync(Course.CurrencyId.Value).Result;
            //if (mappedproperty.CourseWeightId.HasValue && mappedproperty.CourseWeightId.Value > 0)
            //{

            //    mappedproperty.CourseWeight = _unitOfWork.Repository<CourseWeight>().GetByIdAsync(mappedproperty.CourseWeightId.Value).Result;
            //    if (mappedproperty.CourseWeight.UnitId > 0)
            //        mappedproperty.CourseWeight.Unit = _unitOfWork.Repository<SchoolV01.Domain.Entities.GeneralSettings.Unit>().GetByIdAsync(mappedproperty.CourseWeight.UnitId).Result;
            //    if (mappedproperty.CourseWeight.CurrencyId > 0)
            //        mappedproperty.CourseWeight.Currency = _unitOfWork.Repository<Currency>().GetByIdAsync(mappedproperty.CourseWeight.CurrencyId).Result;

            //}
            return await Result<GetCourseOfferByIdResponse>.SuccessAsync(mappedproperty);
        }
    }
}