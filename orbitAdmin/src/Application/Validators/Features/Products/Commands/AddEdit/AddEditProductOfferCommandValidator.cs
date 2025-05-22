using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Products.Commands.AddEdit;


namespace SchoolV01.Application.Validators.Features.Products.Commands.AddEdit
{
    public class AddEditProductOfferCommandValidator : AbstractValidator<AddEditProductOfferCommand>
    {
        public AddEditProductOfferCommandValidator(IStringLocalizer<AddEditProductOfferCommand> localizer)
        {
            RuleFor(request => request.DiscountRatio).GreaterThan(0).WithMessage("Discount Ratio is Required")
                 .LessThan(100).WithMessage("Not a Valid Value"); ;
            RuleFor(request => request.StartDate).NotNull().WithMessage("Start Date is Required")
                .LessThanOrEqualTo(p => p.EndDate).WithMessage("Start Date must be less than End Date");
            RuleFor(request => request.EndDate).NotNull().WithMessage("End Date is Required")
                 .GreaterThanOrEqualTo(p => p.StartDate).WithMessage("End Date must be grater than Start Date");
        }
    }
}
