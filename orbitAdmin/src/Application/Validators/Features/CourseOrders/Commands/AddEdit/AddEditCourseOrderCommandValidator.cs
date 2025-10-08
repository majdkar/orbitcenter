using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.Courses.Commands.AddEdit
{
    public class AddEditCourseOrderCommandValidator : AbstractValidator<AddEditCourseOrderCommand>
    {
        public AddEditCourseOrderCommandValidator(IStringLocalizer<AddEditCourseOrderCommandValidator> localizer)
        {
            RuleFor(request => request.CourseId)
                .NotNull().WithMessage(x => localizer["Course is required!"]);
            RuleFor(request => request.ClientId)
                .NotNull().WithMessage(x => localizer["Client is required!"]);  
   
        }
    }
}