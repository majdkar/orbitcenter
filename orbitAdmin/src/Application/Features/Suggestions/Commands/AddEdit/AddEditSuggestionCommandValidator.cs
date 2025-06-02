using FluentValidation;
using Microsoft.Extensions.Localization;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using SchoolV01.Domain.Enums;
using System.Text.RegularExpressions;

namespace SchoolV01.Application.Validators.Features.Suggestions.Commands.AddEdit { 
    public class AddEditSuggestionCommandValidator : AbstractValidator<AddEditSuggestionCommand>
{
    public AddEditSuggestionCommandValidator(IStringLocalizer<AddEditSuggestionCommandValidator> localizer)
    {
        When(x => x.Type == SuggestionType.Appointment,() => {

            RuleFor(request => request.AppointmentDate).NotEmpty().NotNull().WithMessage(x => localizer["Appointment Date is required!"]);
        });

           
    }




}
}

