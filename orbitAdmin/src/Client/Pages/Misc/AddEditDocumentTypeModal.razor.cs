﻿using System.Threading.Tasks;
using Blazored.FluentValidation;
using SchoolV01.Application.Features.DocumentTypes.Commands;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Misc.DocumentType;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace SchoolV01.Client.Pages.Misc
{
    public partial class AddEditDocumentTypeModal
    {
        [Inject] private IDocumentTypeManager DocumentTypeManager { get; set; }

        [Parameter] public AddEditDocumentTypeCommand AddEditDocumentTypeModel { get; set; } = new();
        [CascadingParameter] private IMudDialogInstance  MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await DocumentTypeManager.SaveAsync(AddEditDocumentTypeModel);
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
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}