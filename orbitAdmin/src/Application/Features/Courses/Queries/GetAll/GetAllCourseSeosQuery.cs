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
    public class GetAllCourseSeosQuery : IRequest<Result<List<GetAllCourseSeosResponse>>>
    {
        public int CourseId { get; set; }
    }

    internal class GetAllCourseSeosQueryHandler : IRequestHandler<GetAllCourseSeosQuery, Result<List<GetAllCourseSeosResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCourseSeosQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllCourseSeosResponse>>> Handle(GetAllCourseSeosQuery request, CancellationToken cancellationToken)
        {

            var CourseSeos = _unitOfWork.Repository<CourseSeo>().Entities.Where(x => x.CourseId == request.CourseId);
            var mappedSeos = _mapper.Map<List<GetAllCourseSeosResponse>>(CourseSeos);
         

            return await Result<List<GetAllCourseSeosResponse>>.SuccessAsync(mappedSeos);
        }
    }
}