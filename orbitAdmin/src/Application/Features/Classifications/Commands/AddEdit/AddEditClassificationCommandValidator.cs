using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Classifications.Commands;

namespace SchoolV01.Application.Validators.Features.ServiceTypes.Commands
{
    public class AddEditClassificationCommandValidator : AbstractValidator<AddEditClassificationCommand>
    {
        public AddEditClassificationCommandValidator(IStringLocalizer<AddEditClassificationCommand> localizer)
        {
			RuleFor(request => request.NameAr).NotEmpty().WithMessage(x => localizer["Arabic Name is required!"]);

        }
    }
}