using SchoolV01.Application.Features.Products.Commands.AddEdit;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace SchoolV01.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditProductCommandValidator : AbstractValidator<AddEditCompanyProductCommand>
    {
        public AddEditProductCommandValidator(IStringLocalizer<AddEditProductCommandValidator> localizer)
        {
            RuleFor(request => request.NameAr)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Arabic Name is required!"]);
            RuleFor(request => request.NameEn)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["English Name is required!"]);
            //RuleFor(request => request.Price)
            //    .GreaterThanOrEqualTo(1).WithMessage(x => localizer["Price is required!"]);
        }
    }
}