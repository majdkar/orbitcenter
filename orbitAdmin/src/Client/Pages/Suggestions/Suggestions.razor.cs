using SchoolV01.Client.Extensions;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

using SchoolV01.Client.Infrastructure.Managers.Suggestions;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Requests.Suggestions;
using SchoolV01.Domain.Entities.Suggestions;
using System.Threading;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Client.Pages.Suggestions
{
    public partial class Suggestions
    {
        [Inject] private ISuggestionManager SuggestionsManager { get; set; }
       [Parameter] public SuggestionType  Type { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllSuggestionsResponse> _pagedData;
        private MudTable<GetAllSuggestionsResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateSuggestions;
        private bool _canEditSuggestions;
        private bool _canDeleteSuggestions;
        private bool _canExportSuggestions;
        private bool _canSearchSuggestions;
        private bool _loaded;

     


        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateSuggestions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Suggestions.Create)).Succeeded;
            _canEditSuggestions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Suggestions.Edit)).Succeeded;
            _canDeleteSuggestions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Suggestions.Delete)).Succeeded;
            _canExportSuggestions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Suggestions.View)).Succeeded;
            _canSearchSuggestions = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Suggestions.View)).Succeeded;

            //await GetBrandsAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }


        private async Task<TableData<GetAllSuggestionsResponse>> ServerReload(TableState state,CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllSuggestionsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedSuggestionRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await SuggestionsManager.GetAllPagedAsync(request,Type);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                _pagedData = response.Data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }


  



        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
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
                var response = await SuggestionsManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task Details(int id)
        {
            _navigationManager.NavigateTo($"/Suggestion-details/{id}");
        }
       
        
        private async Task ExportToExcel()
        {
            var response = await SuggestionsManager.ExportToExcelAsync(_searchString,Type);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Suggestion).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Suggestions exported"]
                    : _localizer["Filtered Suggestions exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }


        private bool Search(GetAllSuggestionsResponse brand)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (brand.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (brand.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (brand.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}