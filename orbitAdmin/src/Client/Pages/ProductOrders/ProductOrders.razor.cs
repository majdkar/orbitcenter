using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged;

using SchoolV01.Client.Extensions;

using SchoolV01.Client.Infrastructure.Managers.ProductOrders;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Threading;
using System.Globalization;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Domain.Entities.Orders;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;

namespace SchoolV01.Client.Pages.ProductOrders
{
    public partial class ProductOrders
    {
        [Inject] private IProductOrderManager ProductOrderManager { get; set; }
        [Inject] private IProductManager ProductManager { get; set; }

        [Inject] private IPersonManager PersonManager { get; set; }
        [Inject] private ICompanyManager CompanyManager { get; set; }


        private List<GetAllPersonsResponse> _persons = new();

        private List<GetAllCompaniesResponse> _companies = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");

        public string OrderNumber { get; set; }
        public string ClientType { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        private IEnumerable<GetAllPagedProductOrdersResponse> _pagedData;
        private MudTable<GetAllPagedProductOrdersResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductOrder;
        private bool _canEditProductOrder;
        private bool _canDeleteProductOrder;
        private bool _canExportProductOrder;
        private bool _canSearchProductOrder;
        private bool _loaded;
        private List<GetAllProductsResponse> _Products = new();



        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductOrder = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Create)).Succeeded;
            _canEditProductOrder = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Edit)).Succeeded;
            _canDeleteProductOrder = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.Delete)).Succeeded;
            _canExportProductOrder = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.View)).Succeeded;
            _canSearchProductOrder = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Orders.View)).Succeeded;

            _loaded = true;
            await LoadProduct();
            await LoadCompanies();
            await LoadPerson();

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

        }


        private async Task LoadCompanies()
        {
            var data = await CompanyManager.GetAllAcceptedAsync();
            if (data.Succeeded)
            {
                _companies = data.Data.ToList();
            }
        }

        private async Task LoadPerson()
        {
            var data = await PersonManager.GetAllAsync();
            if (data.Succeeded)
            {
                _persons = data.Data.ToList();
            }
        }



        private async Task LoadProduct()
        {

            var data = await ProductManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Products = data.Data.ToList();
            }
        }


        private async Task<IEnumerable<int>> SearchCompanies(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _companies.Select(x => x.ClientId);

            return _companies.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.ClientId);
        }

        private async Task<IEnumerable<int>> SearchPersons(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _persons.Select(x => x.ClientId);

            return _persons.Where(x => x.FullName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.FullNameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.ClientId);
        }
        string PersonToString(int id)
        {
            var student = _persons.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.FullName} - {student.FullNameEn}";
        }
        string CompanyToString(int id)
        {
            var student = _companies.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameAr} - {student.NameEn}";
        }

        private async Task<IEnumerable<int>> SearchProducts(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Products.Select(x => x.Id);

            return _Products.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        string ProductToString(int id)
        {
            var student = _Products.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }

        private async Task FilterData()
        {
            await _table.ReloadServerData();
        }

        private async Task<TableData<GetAllPagedProductOrdersResponse>> ServerReload(TableState state,CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductOrdersResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedProductOrdersRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };

           

            var response = await ProductOrderManager.GetAllPagedSearchProductOrdersAsync(request, OrderNumber, ClientId, ProductId, FromPrice, ToPrice);
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
            OrderNumber = null;

            ClientId = 0;
            ProductId = 0;
             FromPrice = 0;
            ToPrice = 0;
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await ProductOrderManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(ProductOrder).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["ProductOrder exported"]
                    : _localizer["Filtered ProductOrder exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToDetails(int ProductOrderId)
        {
            _navigationManager.NavigateTo($"/ProductOrder-details/{ProductOrderId}");
        }

        private string RedirectToViewDetails(int ProductOrderId)
        {
            return $"/view-details/{ProductOrderId}";
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
                var response = await ProductOrderManager.DeleteAsync(id);
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

       private async Task InvokeDetailsModal(int id = 0)
        {
            _navigationManager.NavigateTo($"/ProductOrderDetails/{id}");
        }
    }
}
