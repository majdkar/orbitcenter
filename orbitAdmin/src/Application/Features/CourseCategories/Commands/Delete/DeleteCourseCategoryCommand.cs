using MediatR;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Core.Entities;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Application.Features.CourseCategories.Commands.Delete
{
    public class DeleteCourseCategoryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    internal class DeleteCourseCategoryCommandHandler : IRequestHandler<DeleteCourseCategoryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteCourseCategoryCommandHandler> _localizer;

        public DeleteCourseCategoryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteCourseCategoryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteCourseCategoryCommand command, CancellationToken cancellationToken)
        {
            var CourseCategory = await _unitOfWork.Repository<CourseCategory>().GetByIdAsync(command.Id);
            if (CourseCategory != null)
            {
                CourseCategory.Deleted = true;
                await _unitOfWork.Repository<CourseCategory>().DeleteAsync(CourseCategory);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllCourseCategoriesCacheKey);
                return await Result<int>.SuccessAsync(CourseCategory.Id, _localizer["Course Category Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Course Category Not Found!"]);
            }
        }
    }
}
