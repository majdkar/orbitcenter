using AutoMapper;
using MediatR;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Core.Entities;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;

namespace SchoolV01.Application.Features.Courses.Queries.GetById
{
    public class GetCourseCategoriesByIdQuery : IRequest<Result<GetCourseCategoriesByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseCategoriesByIdQueryHandler : IRequestHandler<GetCourseCategoriesByIdQuery, Result<GetCourseCategoriesByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseCategoriesByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetCourseCategoriesByIdResponse>> Handle(GetCourseCategoriesByIdQuery query, CancellationToken cancellationToken)
        {
            var Course = await _unitOfWork.Repository<CourseCategory>().GetByIdAsync(query.Id);
            var mappedCourse = _mapper.Map<GetCourseCategoriesByIdResponse>(Course);
            return await Result<GetCourseCategoriesByIdResponse>.SuccessAsync(mappedCourse);
        }
    }
}