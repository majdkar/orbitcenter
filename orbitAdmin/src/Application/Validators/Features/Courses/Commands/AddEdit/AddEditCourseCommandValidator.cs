using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.Courses.Commands.AddEdit
{
    public class AddEditCourseCommandValidator : AbstractValidator<AddEditCompanyCourseCommand>
    {
        public AddEditCourseCommandValidator(IStringLocalizer<AddEditCourseCommandValidator> localizer)
        {
            RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
            RuleFor(request => request.NameEn)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);  
            RuleFor(request => request.EndpointEn)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Endpoint En is required!"]);
            //RuleFor(request => request.Price)
            //    .GreaterThanOrEqualTo(1).WithMessage(x => localizer["Price is required!"]);
        }
    }
}