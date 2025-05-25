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
            //RuleFor(request => request.FullNameEn)
            //   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
            RuleFor(request => request.FullNameAr)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
            //RuleFor(request => request.Email)
            //   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email is required"])
            //   .EmailAddress().WithMessage(x => localizer["Email is not correct"]);

        }
    }
}
