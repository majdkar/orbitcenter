using SchoolV01.Client.Extensions;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using SchoolV01.Application.Requests;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using SchoolV01.Client.Infrastructure.Managers.Suggestions;
using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using Microsoft.JSInterop;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;

namespace SchoolV01.Client.Pages.Suggestions
{
    public partial class DetailsSuggestions
    {
        [Inject] private ISuggestionManager SuggestionManager { get; set; }
        [Parameter] public int Id { get; set; } = 0;

        [Parameter] public AddEditSuggestionCommand AddEditSuggestionModel { get; set; } = new();

        [Parameter] public AddEditReplyCommand AddEditReplyModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }


        public GetSuggestionByIdResponse Model;


        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        public async void RedirectToSuggestionPage()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);

        }






        private async Task SaveAsync()
        {
            if (Model.Id > 0)
            {
              AddEditReplyModel.Id = Model.Id;
              AddEditReplyModel.Reply = string.IsNullOrEmpty(Model.Reply) ?  AddEditSuggestionModel.Reply : Model.Reply;
            var response = await SuggestionManager.SaveReplyAsync(AddEditReplyModel);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    RedirectToSuggestionPage();
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
        }





        protected override async Task OnInitializedAsync()
        {
            
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (Id != 0)
            {
                var data = await SuggestionManager.GetByIdAsync(Id);
                if (data.Succeeded)
                {
                    Model = data.Data;
                    //AddEditSuggestionModel = new AddEditSuggestionCommand
                    //{
                    //    Id = response.Id,
                    //    Description = response.Description,
                    //    Email = response.Email,
                    //    UserName = response.UserName,
                    //    Reply = response.Reply,
                    //    Mobile = response.Mobile,
                         
                    //};
                }
            }
            else { AddEditSuggestionModel.Id = 0; }

        }
      
    }
}