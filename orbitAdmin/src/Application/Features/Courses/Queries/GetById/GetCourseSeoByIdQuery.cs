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
using SchoolV01.Application.Specifications.Courses;
using SchoolV01.Core.Entities;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;

namespace SchoolV01.Application.Features.Courses.Queries.GetById
{
    public class GetCourseSeoByIdQuery : IRequest<Result<GetCourseSeoByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseSeoByIdQueryHandler : IRequestHandler<GetCourseSeoByIdQuery, Result<GetCourseSeoByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseSeoByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseSeoByIdResponse>> Handle(GetCourseSeoByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = new CourseSeoByIdFilterSpecification(query.Id);
            Expression<Func<CourseSeo, GetCourseSeoByIdResponse>> expression = e => new GetCourseSeoByIdResponse
            {
                Id = e.Id,
               
                MetaTitleAr = e.MetaTitleAr,
                MetaTitleEn = e.MetaTitleEn,
                MetaTitleGe = e.MetaTitleGe,
               
                MetaNameAr = e.MetaNameAr,
                MetaNameEn = e.MetaNameEn,
                MetaNameGe = e.MetaNameGe,
               
                MetaRobots = e.MetaRobots,
               
                CourseId = e.CourseId,
                
                MetaUrlAr = e.MetaUrlAr,
                 MetaUrlEn = e.MetaUrlEn,
                 MetaUrlGe = e.MetaUrlGe,
                
                MetaKeywordsAr = e.MetaKeywordsAr,
                 MetaKeywordsEn = e.MetaKeywordsEn,
                 MetaKeywordsGe = e.MetaKeywordsGe,

                 MetaDescriptionsAr = e.MetaDescriptionsAr,
                 MetaDescriptionsEn = e.MetaDescriptionsEn,
                 MetaDescriptionsGe = e.MetaDescriptionsGe,


                  ImageAlt1Ar = e.ImageAlt1Ar,
                  ImageAlt1En = e.ImageAlt1En,
                  ImageAlt1Ge = e.ImageAlt1Ge,

                  ImageAlt2Ar = e.ImageAlt2Ar,
                  ImageAlt2En = e.ImageAlt2En,
                  ImageAlt2Ge = e.ImageAlt2Ge,

                  ImageAlt3Ar = e.ImageAlt3Ar,
                  ImageAlt3En = e.ImageAlt3En,
                  ImageAlt3Ge = e.ImageAlt3Ge,

                  ImageAlt4Ar = e.ImageAlt4Ar,
                  ImageAlt4En = e.ImageAlt4En,
                  ImageAlt4Ge = e.ImageAlt4Ge,


                  
            };

            var CourseSeo = await _unitOfWork.Repository<CourseSeo>().Entities
            .Specify(filter)
            .Select(expression)
            .FirstOrDefaultAsync();

            return await Result<GetCourseSeoByIdResponse>.SuccessAsync(CourseSeo);
        }
    }
}