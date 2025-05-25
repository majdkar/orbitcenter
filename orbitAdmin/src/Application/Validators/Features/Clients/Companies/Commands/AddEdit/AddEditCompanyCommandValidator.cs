using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using System.Text.RegularExpressions;

namespace SchoolV01.Application.Validators.Features.Clients.Companies.Commands.AddEdit
{
    public class AddEditCompanyCommandValidator : AbstractValidator<AddEditCompanyCommand>
    {
        public AddEditCompanyCommandValidator(IStringLocalizer<AddEditCompanyCommandValidator> localizer)
        {
            //RuleFor(request => request.NameEn)
            //   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Full English Name is required!"]);
            RuleFor(request => request.NameAr)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Full Arabic Name is required!"]);
            RuleFor(request => request.Phone)
               .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Phone is required!"]);
            //RuleFor(request => request.Email)
            //   .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email is required"])
            //   .EmailAddress().WithMessage(x => localizer["Email is not correct"]);
         
        }
    }
}
