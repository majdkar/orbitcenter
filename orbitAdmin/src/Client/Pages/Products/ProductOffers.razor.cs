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

namespace SchoolV01.Client.Pages.Products
{
    public partial  class ProductOffers
    {
        [Parameter] public int ProductId { get; set; }
        [Parameter] public bool DisableAddButton { get; set; } = true;
        [Parameter] public decimal OldPrice { get; set; } = 0;

        [Inject] private IProductOfferManager productOfferManager { get; set; }
        [Parameter] public AddEditCompanyProductCommand AddEditCompanyProductModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllProductOffersResponse> _offers = new();
        private GetAllProductOffersResponse _offer = new();

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProductOffer;
        private bool _canEditProductOffer;
        private bool _canDeleteProductOffer;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProductOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProductOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProductOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;

            await LoadOffersAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadOffersAsync()
        {
            if (ProductId != 0)
            {
                var response = await productOfferManager.GetAllByProductAsync(ProductId);
                if (response.Succeeded)
                {
                    _offers = response.Data.ToList();
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
                var response = await productOfferManager.DeleteAsync(id);
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
            _offers = new List<GetAllProductOffersResponse>();
            await LoadOffersAsync();
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0) // update
            {
                _offer = _offers.FirstOrDefault(c => c.Id == id);
                if (_offer != null)
                {
                    parameters.Add(nameof(AddEditProductOfferModal.AddEditProductOfferModel), new AddEditProductOfferCommand
                    {
                        Id = _offer.Id,
                        ProductId = ProductId,
                        DiscountRatio = _offer.DiscountRatio,
                        NewPrice = _offer.NewPrice,
                        StartDate = _offer.StartDate,
                        EndDate = _offer.EndDate,
                        OldPrice = OldPrice,
                     
                    });
                    parameters.Add(nameof(AddEditProductOfferModal.AddEditCompanyProductModel), AddEditCompanyProductModel);

                }
            }
            else // add
            {
                parameters.Add(nameof(AddEditProductOfferModal.AddEditProductOfferModel), new AddEditProductOfferCommand
                {
                    Id = 0,
                    ProductId = ProductId,
                    OldPrice = OldPrice,
                    NewPrice = OldPrice,
                    DiscountRatio = 0
                    
                });

                parameters.Add(nameof(AddEditProductOfferModal.AddEditCompanyProductModel), AddEditCompanyProductModel);
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = _dialogService.Show<AddEditProductOfferModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reset();
            }
        }

    }
}
