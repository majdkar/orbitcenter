using MediatR;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Extensions;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Application.Specifications.Courses;
using SchoolV01.Domain.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Queries.GetActiveCourseOffer
{
    public class GetAllActiveCourseOffersQuery : IRequest<List<GetAllActiveCourseOffersResponse>>
    {

    }
    internal class GetAllCoursesQueryHandler : IRequestHandler<GetAllActiveCourseOffersQuery, List<GetAllActiveCourseOffersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllCoursesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<GetAllActiveCourseOffersResponse>> Handle(GetAllActiveCourseOffersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<CourseOffer, GetAllActiveCourseOffersResponse>> expression = e => new GetAllActiveCourseOffersResponse
            {
                Id = e.Id,
                CourseId = e.CourseId,
                CourseNameAr = e.Course.NameAr,
                CourseNameEn = e.Course.NameEn,
                 
                Price = e.Course.Price,
                //CompanyId = e.Course.CompanyId,
                //CompanyNameAr = e.Course.Company.NameAr,
                //CompanyNameEn = e.Course.Company.NameEn,
                OldPrice = e.Course.Price,
                NewPrice = e.NewPrice,
                DiscountRatio = e.DiscountRatio,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            };
            var offerFilterSpec = new AllActiveCourseOfferFilterSpecification();
            var data = await _unitOfWork.Repository<CourseOffer>().Entities
                  .Specify(offerFilterSpec)
                  .Select(expression)
                  .ToListAsync();
            return data;
        }
    }
}