using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Shared.Constants.Application;

namespace SchoolV01.Client.Pages.Products
{
    public partial class AddEditProductOfferModal
    {
        [Inject] private IProductOfferManager ProductOfferManager { get; set; }

        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }

        [Parameter] public AddEditProductOfferCommand AddEditProductOfferModel { get; set; } = new();
        [Parameter] public AddEditCompanyProductCommand AddEditCompanyProductModel { get; set; } = new();

        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
         public int ParentCategoryId { get; set; } = 0;
         public int SubCategoryId { get; set; } = 0;
         public int SubSubCategoryId { get; set; } = 0;
         public int SubSubSubCategoryId { get; set; } = 0;
         public int DefaultCategoryId { get; set; } = 0;

        public int ProductId { get; set; } = 0;
        private List<GetAllProductCategoriesResponse> _allCategories = new();
        private List<GetAllProductCategoriesResponse> _parentCategories = new();
        private List<GetAllProductCategoriesResponse> _subCategories = new();
        private List<GetAllProductCategoriesResponse> _subSubCategories = new();
        private List<GetAllProductCategoriesResponse> _subSubSubCategories = new();
        private bool _isProcessing = false;
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            _isProcessing = true;
            var response = await ProductOfferManager.SaveAsync(AddEditProductOfferModel);
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
            _isProcessing = false;
        }

        protected override async Task OnInitializedAsync()
        {
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            ParentCategoryId = AddEditCompanyProductModel.ProductParentCategoryId ?? 0;
            SubCategoryId = AddEditCompanyProductModel.ProductSubCategoryId ?? 0;
            SubSubCategoryId = AddEditCompanyProductModel.ProductSubSubCategoryId ?? 0;
            SubSubSubCategoryId = AddEditCompanyProductModel.ProductSubSubSubCategoryId ?? 0;
            await LoadProductParentCategories();
           await LoadProductParentCategorySons();
           await LoadProductSubCategorySons();
           await LoadProductSubSubCategorySons();
        }
        private async Task LoadProductParentCategories()
        {
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _parentCategories = data.Data.Where(x => (x.ParentCategoryId == null || x.ParentCategoryId == 0)).ToList();
            }
        }

        private async Task LoadProductParentCategorySons()
        {

            if (ParentCategoryId == 0)
                return;
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {

                _subCategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId ).ToList();
            }
            DefaultCategoryId = ParentCategoryId;
            //_sizes.Clear();

            //await LoadSizes(ParentCategoryId);
            //await LoadProductSizeColors(ProductId, ParentCategoryId);


        }

        private async Task LoadProductSubCategorySons()
        {
            if (SubCategoryId == 0)
            {
                DefaultCategoryId = ParentCategoryId;
                return;
            }
            //_subSubCategories.Clear();
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubCategories = data.Data.Where(x => x.ParentCategoryId == SubCategoryId ).ToList();
            }
            DefaultCategoryId = SubCategoryId;
            //_sizes.Clear();

            //await LoadSizes(SubCategoryId);
            //await LoadProductSizeColors(ProductId, SubCategoryId);
        }
        private async Task LoadProductSubSubCategorySons()
        {
            if (SubSubCategoryId == 0)
            {
                DefaultCategoryId = SubCategoryId;
                return;
            }
            //_subSubSubCategories.Clear();
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubSubCategories = data.Data.Where(x => x.ParentCategoryId == SubSubCategoryId).ToList();
            }
            DefaultCategoryId = SubSubCategoryId;
            //_sizes.Clear();

            //await LoadSizes(SubSubCategoryId);
            //await LoadProductSizeColors(ProductId, SubSubCategoryId);

        }

        private async void UpdateDefaultCategoryId()
        {
            if (SubSubSubCategoryId == 0)
            {
                DefaultCategoryId = SubSubCategoryId;
                return;
            }
            DefaultCategoryId = SubSubSubCategoryId;
        }

        private void CalculateNewPrice()
        {
            AddEditProductOfferModel.NewPrice = Math.Round((decimal)(AddEditProductOfferModel.OldPrice - (AddEditProductOfferModel.OldPrice * AddEditProductOfferModel.DiscountRatio / 100)), 2);
        }

     


    }
}

