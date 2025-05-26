using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Application.Features.CourseTypes.Commands
{
    public class DeleteCourseTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteCourseTypeCommandHandler : IRequestHandler<DeleteCourseTypeCommand, Result<int>>
    {

        private readonly IStringLocalizer<DeleteCourseTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteCourseTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseTypeCommand command, CancellationToken cancellationToken)
        {
            var CourseType = await _unitOfWork.Repository<CourseType>().Entities.FirstOrDefaultAsync(x => x.Id == command.Id);
            if (CourseType != null)
            {
                await _unitOfWork.Repository<CourseType>().DeleteAsync(CourseType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(CourseType));
                return await Result<int>.SuccessAsync(CourseType.Id, _localizer["CourseType Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["CourseType Not Found!"]);
            }
        }
    }
}
