using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.CourseTypes.Commands;

namespace SchoolV01.Application.Validators.Features.ServiceTypes.Commands
{
    public class AddEditCourseTypeCommandValidator : AbstractValidator<AddEditCourseTypeCommand>
    {
        public AddEditCourseTypeCommandValidator(IStringLocalizer<AddEditCourseTypeCommand> localizer)
        {
			RuleFor(request => request.NameAr).NotEmpty().WithMessage(x => localizer["Arabic Name is required!"]);

        }
    }
}