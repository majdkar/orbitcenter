using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;

namespace SchoolV01.Client.Pages.Products
{
    public partial  class ProductSeos
    {
        [Parameter] public int ProductId { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public bool DisableAddButton { get; set; } = false;
        [Parameter] public decimal OldPrice { get; set; } = 0;

        [Inject] private IProductSeoManager productSeoManager { get; set; }
        [Parameter] public AddEditCompanyProductCommand AddEditCompanyProductModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllProductSeosResponse> _Seos = new();
        private GetAllProductSeosResponse _Seo = new();

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductSeo;
        private bool _canEditProductSeo;
        private bool _canDeleteProductSeo;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProductSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProductSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;

            await LoadSeosAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadSeosAsync()
        {
            if (ProductId != 0)
            {
                var response = await productSeoManager.GetAllByProductAsync(ProductId);
                if (response.Succeeded)
                {
                    _Seos = response.Data.ToList();
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
                var response = await productSeoManager.DeleteAsync(id);
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

        private async Task Reset()
        {
            _Seos = new List<GetAllProductSeosResponse>();
            await LoadSeosAsync();
        }

        private async Task InvokeModal(int id = 0,int productId = 0)
        {
            _navigationManager.NavigateTo($"/seo-details/{id}/{productId}");

        }

    }
}
