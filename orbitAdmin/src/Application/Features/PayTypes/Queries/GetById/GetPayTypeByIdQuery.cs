using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.PayTypes.Queries.GetById
{
    public class GetPayTypeByIdQuery : IRequest<Result<GetPayTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetPayTypeByIdQueryHandler : IRequestHandler<GetPayTypeByIdQuery, Result<GetPayTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPayTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPayTypeByIdResponse>> Handle(GetPayTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var PayType = await _unitOfWork.Repository<PayType>().GetByIdAsync(query.Id);
            var mappedPayType = _mapper.Map<GetPayTypeByIdResponse>(PayType);
            return await Result<GetPayTypeByIdResponse>.SuccessAsync(mappedPayType);
        }
    }
}