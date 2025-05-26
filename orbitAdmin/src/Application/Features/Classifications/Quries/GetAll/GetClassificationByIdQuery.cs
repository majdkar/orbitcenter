using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Classifications.Queries
{
    public class GetClassificationByIdQuery : IRequest<Result<GetAllClassificationsResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetClassificationByIdQueryHandler : IRequestHandler<GetClassificationByIdQuery, Result<GetAllClassificationsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClassificationByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllClassificationsResponse>> Handle(GetClassificationByIdQuery query, CancellationToken cancellationToken)
        {
            var Classification = await _unitOfWork.Repository<Classification>().GetByIdAsync(query.Id);
            var mappedClassification = _mapper.Map<GetAllClassificationsResponse>(Classification);
            return await Result<GetAllClassificationsResponse>.SuccessAsync(mappedClassification);
        }
    }
}
