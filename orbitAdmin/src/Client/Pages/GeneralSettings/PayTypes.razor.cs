using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.PayType;
using SchoolV01.Application.Features.PayTypes.Commands.AddEdit;
using Microsoft.AspNetCore.Authorization;
using SchoolV01.Client.Extensions;
using System.Linq;


namespace SchoolV01.Client.Pages.GeneralSettings
{
    public partial class PayTypes
    {
        [Inject] private IPayTypeManager PayTypeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllPayTypesResponse> _PayTypeList = new();
        private GetAllPayTypesResponse _PayType = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePayTypes;
        private bool _canEditPayTypes;
        private bool _canDeletePayTypes;
        private bool _canExportPayTypes;
        private bool _canSearchPayTypes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreatePayTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PayTypes.Create)).Succeeded;
            _canEditPayTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PayTypes.Edit)).Succeeded;
            _canDeletePayTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PayTypes.Delete)).Succeeded;
            _canExportPayTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PayTypes.View)).Succeeded;
            _canSearchPayTypes = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.PayTypes.View)).Succeeded;

            await GetPayTypesAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        public async void Back()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }

        private async Task GetPayTypesAsync()
        {
            var response = await PayTypeManager.GetAllAsync();
            if (response.Succeeded)
            {
                _PayTypeList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await PayTypeManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    await Reset();
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task ExportToExcel()
        {
            var response = await PayTypeManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(PayTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["PayTypes exported"]
                    : _localizer["Filtered PayTypes exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                _PayType = _PayTypeList.FirstOrDefault(c => c.Id == id);
                if (_PayType != null)
                {
                    parameters.Add(nameof(AddEditPayTypeModal.AddEditPayTypeModel), new AddEditPayTypeCommand
                    {
                        Id = _PayType.Id,
                        NameAr = _PayType.NameAr,
                        NameEn = _PayType.NameEn,
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true};
            var dialog = _dialogService.Show<AddEditPayTypeModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reset();
            }
        }

        private async Task Reset()
        {
            _PayType = new GetAllPayTypesResponse();
            await GetPayTypesAsync();
        }

        private bool Search(GetAllPayTypesResponse PayType)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (PayType.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (PayType.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }



            /**/
            return false;
        }

    }
}
