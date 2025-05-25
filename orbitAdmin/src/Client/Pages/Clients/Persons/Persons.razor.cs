using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAllPaged;
using SchoolV01.Application.Requests.Clients.Persons;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Client.Infrastructure.Managers.Products;
using System.Linq;
using System.Threading;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;

namespace SchoolV01.Client.Pages.Clients.Persons
{
    public partial class Persons
    {
        [Inject] private IPersonManager PersonManager { get; set; }
        public string PersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        private IEnumerable<GetAllPersonsResponse> _pagedData;
        private MudTable<GetAllPersonsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreatePersons;
        private bool _canViewPersons;
        private bool _canEditPersons;
        private bool _canDeletePersons;
        private bool _canExportPersons;
        private bool _canSearchPersons;
        private bool _loaded;
        private bool _processing = false;
        public bool checkedForDelete { get; set; } = false;
        private List<string> clickedEvents = new();
        private HashSet<GetAllPersonsResponse> selectedItems = new HashSet<GetAllPersonsResponse>();

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewPersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.View)).Succeeded;
            _canCreatePersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.Create)).Succeeded;
            _canEditPersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.Edit)).Succeeded;
            _canDeletePersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.Delete)).Succeeded;
            _canExportPersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.View)).Succeeded;
            _canSearchPersons = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Person.View)).Succeeded;

            _loaded = true;

        }

        private async Task<TableData<GetAllPersonsResponse>> ServerReload(TableState state, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPersonsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                if (PhoneNumber.Contains('+'))
                {
                    PhoneNumber = PhoneNumber.Replace('+', '0');
                }
            }
            var request = new GetAllPagedPersonsRequest { PersonName = PersonName, PhoneNumber = PhoneNumber, Email = Email, PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await PersonManager.GetPersonsAsync(request);
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
            Email = null;
            PhoneNumber = null;
            PersonName = null;
            _searchString = text;
            _table.ReloadServerData();
        }
          private void OnAdvancedSearch()
        {
            _table.ReloadServerData();
        }
        private async Task ExportToExcel()
        {
            var response = await PersonManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Persons).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Persons exported"]
                    : _localizer["Filtered Persons exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToAddEditPage(int personId = 0)
        {
            _navigationManager.NavigateTo($"/individual-details/{personId}");
        }



        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true};
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await PersonManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
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
        public void RowClicked(TableRowClickEventArgs<GetAllPersonsResponse> p)
        {
            _navigationManager.NavigateTo($"/individual-details/{p.Item.Id}");
        }
        //private void RedirectToFinancialEventsPage(int companyId)
        //{
        //    _navigationManager.NavigateTo("/finantcialevents");
        //}
    }
}
