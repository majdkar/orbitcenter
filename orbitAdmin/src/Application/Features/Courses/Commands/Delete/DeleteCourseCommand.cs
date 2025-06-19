using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Commands.Delete
{
    public class DeleteCourseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCourseCommandHandler> _localizer;

        public DeleteCourseCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
        {
            var Course = await _unitOfWork.Repository<Course>().GetByIdAsync(command.Id);
            if (Course != null)
            {
                Course.Deleted = true;
                await _unitOfWork.Repository<Course>().DeleteAsync(Course);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(Course.Id, _localizer["Course Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Not Found!"]);
            }
        }
    }
}