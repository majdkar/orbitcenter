using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using System.Text.RegularExpressions;

namespace SchoolV01.Application.Validators.Features.Clients.Persons.Commands.AddEdit
{
    public class AddEditPersonCommandValidator : AbstractValidator<AddEditPersonCommand>
    {
        public AddEditPersonCommandValidator(IStringLocalizer<AddEditPersonCommandValidator> localizer)
        {
            RuleFor(request => request.Mobile1)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Mobile is required!"]);


            RuleFor(request => request.FullNameEn)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);

            RuleFor(request => request.ClassificationId)
               .NotEmpty().NotNull().WithMessage(x => localizer["Classification is required!"]);


        }
    }
}
