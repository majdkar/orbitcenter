using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.CourseTypes.Queries
{
    public class GetCourseTypeByIdQuery : IRequest<Result<GetAllCourseTypesResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetCourseTypeByIdQueryHandler : IRequestHandler<GetCourseTypeByIdQuery, Result<GetAllCourseTypesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCourseTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllCourseTypesResponse>> Handle(GetCourseTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var CourseType = await _unitOfWork.Repository<CourseType>().GetByIdAsync(query.Id);
            var mappedCourseType = _mapper.Map<GetAllCourseTypesResponse>(CourseType);
            return await Result<GetAllCourseTypesResponse>.SuccessAsync(mappedCourseType);
        }
    }
}
