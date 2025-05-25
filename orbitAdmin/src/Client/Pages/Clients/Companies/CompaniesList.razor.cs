using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Requests.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Shared.Constants.Clients;
using System.Threading;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Application.Features.Countries.Queries;

namespace SchoolV01.Client.Pages.Clients.Companies
{
    public partial class CompaniesList
    {
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private ICountryManager CountryManager { get; set; }
        //[Inject] private IProductManager ProductManager { get; set; }
        //[Inject] private IFinalOrdinaryOrderManager FinalOrdinaryOrderManager { get; set; }
        //[Inject] private IPriceOfferManager PriceOfferManager { get; set; }

        [Parameter] public string Status { get; set; } = ClientStatusEnum.Accepted.ToString();
        //[Parameter] public string ActivityCode { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; } = 0;
        private IEnumerable<GetAllCompaniesResponse> _pagedData;
        private MudTable<GetAllCompaniesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private List<GetAllCountriesResponse> _Countrys = new();
        //private List<GetAllProductsResponse> _products = new();
        //private List<GetAllFinalOrdinaryOrdersResponse> _finalOrdinaryOrders = new();
        //private List<GetAllPriceOffersResponse> _priceOffers = new();

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCompanies;
        private bool _canViewCompanies;
        private bool _canEditCompanies;
        private bool _canDeleteCompanies;
        private bool _canExportCompanies;
        private bool _canSearchCompanies;
        private bool _loaded;


        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.View)).Succeeded;
            _canCreateCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Create)).Succeeded;
            _canEditCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Edit)).Succeeded;
            _canDeleteCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.Delete)).Succeeded;
            _canExportCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.View)).Succeeded;
            _canSearchCompanies = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Companies.View)).Succeeded;
            await LoadCountrysAsync();
            //_snackBar.Add(Status, Severity.Error);
            //await LoadProductsAsync();
            //await LoadFinalOrdinaryOrdersAsync();
            //await LoadPriceOffersAsync();
            _loaded = true;
            await base.OnInitializedAsync();

        }

        protected override async Task OnParametersSetAsync()
        {
            if (_table != null)
                await _table.ReloadServerData();
        }

        private async Task LoadCountrysAsync()
        {

            var data = await CountryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Countrys = data.Data;
            }
        }

        //private async Task LoadProductsAsync()
        //{

        //    var data = await ProductManager.GetAllAsync();
        //    if (data.Succeeded)
        //    {
        //        _products = data.Data;
        //    }
        //}
        //private async Task LoadFinalOrdinaryOrdersAsync()
        //{

        //    var data = await FinalOrdinaryOrderManager.GetAllAsync();
        //    if (data.Succeeded)
        //    {
        //        _finalOrdinaryOrders = data.Data;
        //    }
        //}
        //private async Task LoadPriceOffersAsync()
        //{

        //    var data = await PriceOfferManager.GetAllPagedAsync(new GetAllPriceOffersRequest());
        //    if (data.Succeeded)
        //    {
        //        _priceOffers = data.Data;
        //    }
        //}

        private async Task<TableData<GetAllCompaniesResponse>> ServerReload(TableState state, CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllCompaniesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }
            var request = new GetAllPagedCompaniesByTypeRequest { Status=Status,CompanyName = CompanyName, Email = Email, PhoneNumber = PhoneNumber, CountryId = CountryId, PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await CompanyManager.GetAcceptedAsync(request);
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
            CompanyName = null;
            PhoneNumber = null;
            Email = null;
       CountryId  = 0;
        _searchString = text;
            _table.ReloadServerData();
        }
        private void OnAdvancedSearch()
        {
            _table.ReloadServerData();
        }


        private async Task ExportToExcel()
        {
            var response = await CompanyManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Companies).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Companies exported"]
                    : _localizer["Filtered Companies exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToAddEditPage(int companyId = 0)
        {
            _navigationManager.NavigateTo($"/company-details/{companyId}");
        }

        public void RowClicked(TableRowClickEventArgs<GetAllCompaniesResponse> p)
        {
            _navigationManager.NavigateTo($"/company-details/{p.Item.Id}");
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
                var response = await CompanyManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    // await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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

        private async Task Accept(int id)
        {
            string deleteContent = _localizer["ConfirmOperation"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await CompanyManager.AcceptAsync(id);
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

        private async Task Refuse(int id)
        {
            string deleteContent = _localizer["ConfirmOperation"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true};
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Confirm"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var response = await CompanyManager.RefuseAsync(id);
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

    }
}
