using Blazored.FluentValidation;
using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using SchoolV01.Application.Requests;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.PayType;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.GeneralSettings
{
    public partial class AddEditPayTypeModal
    {
        [Inject] private IPayTypeManager PayTypeManager { get; set; }

        [Parameter] public AddEditPayTypeCommand AddEditPayTypeModel { get; set; } = new();
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }
       
        private async Task SaveAsync()
        {
            var response = await PayTypeManager.SaveAsync(AddEditPayTypeModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

       
    }
}
