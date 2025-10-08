using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.CourseOrders.Commands.Delete
{
    public class DeleteCourseOrderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCourseOrderCommandHandler : IRequestHandler<DeleteCourseOrderCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCourseOrderCommandHandler> _localizer;

        public DeleteCourseOrderCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseOrderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseOrderCommand command, CancellationToken cancellationToken)
        {
            var courseOrder = await _unitOfWork.Repository<CourseOrder>().GetByIdAsync(command.Id);
            if (courseOrder != null)
            {
                courseOrder.Deleted = true;
                await _unitOfWork.Repository<CourseOrder>().UpdateAsync(courseOrder);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(courseOrder.Id, _localizer["CourseOrder Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["CourseOrder Not Found!"]);
            }
        }
    }
}