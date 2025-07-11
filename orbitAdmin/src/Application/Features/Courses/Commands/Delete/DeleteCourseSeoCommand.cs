using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.Courses.Commands.Delete
{
    public class DeleteCourseSeoCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCourseSeoCommandHandler : IRequestHandler<DeleteCourseSeoCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCourseSeoCommandHandler> _localizer;

        public DeleteCourseSeoCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseSeoCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseSeoCommand command, CancellationToken cancellationToken)
        {
            var CourseSeo = await _unitOfWork.Repository<CourseSeo>().GetByIdAsync(command.Id);
            if (CourseSeo != null)
            {
                await _unitOfWork.Repository<CourseSeo>().DeleteAsync(CourseSeo);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(CourseSeo.Id, _localizer["Course Seo Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Seo Not Found!"]);
            }
        }
    }
}