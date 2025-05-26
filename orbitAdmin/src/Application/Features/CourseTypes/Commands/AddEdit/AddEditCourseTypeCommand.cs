using AutoMapper;
using SchoolV01.Application.Interfaces.Repositories;
using SchoolV01.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using SchoolV01.Domain.Entities.GeneralSettings;

namespace SchoolV01.Application.Features.CourseTypes.Commands
{
    public class AddEditCourseTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }

        public string NameEn { get; set; }

    }

    internal class AddEditCourseTypeCommandHandler : IRequestHandler<AddEditCourseTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditCourseTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditCourseTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditCourseTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCourseTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var CourseType = _mapper.Map<CourseType>(command);
                await _unitOfWork.Repository<CourseType>().AddAsync(CourseType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(CourseType));
                return await Result<int>.SuccessAsync(CourseType.Id, _localizer["CourseType Saved"]);
            }
            else
            {
                var CourseType = await _unitOfWork.Repository<CourseType>().GetByIdAsync(command.Id);
                if (CourseType != null)
                {
                    CourseType.NameAr = command.NameAr ?? CourseType.NameAr;
                    CourseType.NameEn = command.NameEn ?? CourseType.NameEn;
                

                    await _unitOfWork.Repository<CourseType>().UpdateAsync(CourseType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, nameof(CourseType));
                    return await Result<int>.SuccessAsync(CourseType.Id, _localizer["CourseType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["CourseType Not Found!"]);
                }
            }
        }
    }
}
