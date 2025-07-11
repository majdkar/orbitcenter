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
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Requests.Products;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Threading;
using System.Globalization;

namespace SchoolV01.Client.Pages.Products
{
    public partial class Products
    {
        [Inject] private IProductManager ProductManager { get; set; }

        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");

        public string ProductName { get; set; }

        public int ProductCategoryId { get; set; }

        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public int SubSubCategoryId { get; set; }
        public int SubSubSubCategoryId { get; set; }


        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private MudTable<GetAllPagedProductsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProduct;
        private bool _canEditProduct;
        private bool _canDeleteProduct;
        private bool _canExportProduct;
        private bool _canSearchProduct;
        private bool _loaded;


        private IEnumerable<GetAllProductCategoriesResponse> categories;
        private IEnumerable<GetAllProductCategoriesResponse> Subcategories;
        private IEnumerable<GetAllProductCategoriesResponse> SubSubcategories;
        private IEnumerable<GetAllProductCategoriesResponse> SubSubSubcategories;
        
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;
            _canExportProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;
            _canSearchProduct = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.View)).Succeeded;

            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            await LoadCategories();
            await LoadsubCategories();
            //await LoadsubsubCategories();
        }

        private async Task LoadCategories()
        {
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                categories = data.Data.Where(x => (x.ParentCategoryId == null || x.ParentCategoryId == 0));
            }
        }

  
        private async Task LoadsubCategories()
        {

            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                Subcategories = data.Data.Where(x => x.ParentCategoryId != null);
                SubSubcategories = data.Data.Where(x => x.ParentCategoryId != null);
                SubSubSubcategories = data.Data.Where(x => x.ParentCategoryId != null);
            }
        }
        private async Task LoadsubsubCategories()
        {

            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                SubSubcategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId);
                await FilterData();
            }
        }    
        
        private async Task LoadsubsubsbCategories()
        {

            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                SubSubSubcategories = data.Data.Where(x => x.ParentCategoryId == SubSubCategoryId);
                await FilterData();
            }
        }

        private async Task LoadSubcategory()
        {
            var data = await ProductCategoryManager.GetAllAsync();
            if (data != null)
            {

                Subcategories = data.Data.Where(x => x.ParentCategoryId == CategoryId);
                await FilterData();
            }

        }

        private async Task FilterData()
        {
            await _table.ReloadServerData();
        }

        private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state,CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };

            //if(SubSubSubCategoryId > 0)
            //{
            //    ProductCategoryId = SubSubSubCategoryId;
            //}
            //else if (SubSubCategoryId > 0)
            //{
            //    ProductCategoryId = SubSubCategoryId;

            //}
             if (ParentCategoryId > 0)
            {
                ProductCategoryId = ParentCategoryId;

            }
            else
            { ProductCategoryId = CategoryId;
            }

            var response = await ProductManager.GetAllPagedSearchProductAsync(request, ProductName, CategoryId, ParentCategoryId, SubSubCategoryId, SubSubSubCategoryId, FromPrice
                , ToPrice);
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
            ProductName = null;

            ProductCategoryId = 0;
            CategoryId = 0;
            ParentCategoryId = 0;
            SubSubCategoryId = 0;
            SubSubSubCategoryId = 0;

        FromPrice = 0;

            ToPrice = 0;
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var response = await ProductManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Product).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Product exported"]
                    : _localizer["Filtered Product exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToDetails(int ProductId)
        {
            _navigationManager.NavigateTo($"/Product-details/{ProductId}");
        }

        private string RedirectToViewDetails(int ProductId)
        {
            return $"/view-details/{ProductId}";
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
                var response = await ProductManager.DeleteAsync(id);
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

        private async Task InvokeModalImage(int id = 0)
        {
            _navigationManager.NavigateTo($"/ProductAlbumImage/{id}");
        }   
        
        private async Task InvokeDetailsModal(int id = 0)
        {
            _navigationManager.NavigateTo($"/ProductDetails/{id}");
        }  private async Task InvokeSeosModal(int id = 0,string name = "")
        {
            _navigationManager.NavigateTo($"/ProductSeo/{id}/{name}");
        }
        private async Task InvokeModalOptions(int id = 0)
        {
            _navigationManager.NavigateTo($"/product-options/{id}");
        }
        private async Task InvokeModalSizes(int id = 0)
        {
            _navigationManager.NavigateTo($"/product-weights/{id}");
        }
        private async Task InvokeModalVideo(int id = 0)
        {
            _navigationManager.NavigateTo($"/ProductAlbumVideo/{id}");
        }

    }
}
