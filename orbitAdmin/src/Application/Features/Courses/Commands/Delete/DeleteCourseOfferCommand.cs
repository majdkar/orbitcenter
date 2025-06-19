using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Commands.Delete
{
    public class DeleteCourseOfferCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCourseOfferCommandHandler : IRequestHandler<DeleteCourseOfferCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCourseOfferCommandHandler> _localizer;

        public DeleteCourseOfferCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseOfferCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseOfferCommand command, CancellationToken cancellationToken)
        {
            var CourseOffer = await _unitOfWork.Repository<CourseOffer>().GetByIdAsync(command.Id);
            if (CourseOffer != null)
            {
                await _unitOfWork.Repository<CourseOffer>().DeleteAsync(CourseOffer);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(CourseOffer.Id, _localizer["Course Offer Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Offer Not Found!"]);
            }
        }
    }
}