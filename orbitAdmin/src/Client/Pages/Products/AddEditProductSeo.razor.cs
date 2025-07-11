using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Requests;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Helpers;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Client.Infrastructure.Managers.Identity.Account;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Shared;
using SchoolV01.Shared.Constants;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Constants.Role;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;



namespace SchoolV01.Client.Pages.Products
{
    public partial class AddEditProductSeo
    {

        [Inject] private IProductSeoManager ProductSeoManager { get; set; }



        [Parameter] public int Id { get; set; } = 0;
        [Parameter] public int ProductId { get; set; } = 0;

        private AddEditProductSeoCommand AddEditProductSeoModel { get; set; } = new();
      
        private RegisterRequest _registerUserModel = new();
        private FluentValidationValidator _fluentValidationValidator;

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

     
        private bool _loaded = false;


        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadProductDetails();
            
        }


     

        public async void Back()
        {
            //_navigationManager.NavigateTo($"/menus/{MenuModel.CategoryId}");
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }

        private async Task LoadProductDetails()
        {
            if (Id != 0)
            {
                var data = await ProductSeoManager.GetByIdAsync(Id);

                if (data.Succeeded)
                {
                    var Product = data.Data;

                    AddEditProductSeoModel = new AddEditProductSeoCommand
                    {
                        Id = Product.Id,
                        MetaTitleAr = Product.MetaTitleAr,
                        MetaTitleEn = Product.MetaTitleEn,
                        MetaTitleGe = Product.MetaTitleGe,

                        MetaNameAr = Product.MetaNameAr,
                        MetaNameEn = Product.MetaNameEn,
                        MetaNameGe = Product.MetaNameGe,

                        MetaRobots = Product.MetaRobots,

                        ProductId = Product.ProductId,

                        MetaUrlAr = Product.MetaUrlAr,
                        MetaUrlEn = Product.MetaUrlEn,
                        MetaUrlGe = Product.MetaUrlGe,

                        MetaKeywordsAr = Product.MetaKeywordsAr,
                        MetaKeywordsEn = Product.MetaKeywordsEn,
                        MetaKeywordsGe = Product.MetaKeywordsGe,

                        MetaDescriptionsAr = Product.MetaDescriptionsAr,
                        MetaDescriptionsEn = Product.MetaDescriptionsEn,
                        MetaDescriptionsGe = Product.MetaDescriptionsGe,


                        ImageAlt1Ar = Product.ImageAlt1Ar,
                        ImageAlt1En = Product.ImageAlt1En,
                        ImageAlt1Ge = Product.ImageAlt1Ge,

                        ImageAlt2Ar = Product.ImageAlt2Ar,
                        ImageAlt2En = Product.ImageAlt2En,
                        ImageAlt2Ge = Product.ImageAlt2Ge,

                        ImageAlt3Ar = Product.ImageAlt3Ar,
                        ImageAlt3En = Product.ImageAlt3En,
                        ImageAlt3Ge = Product.ImageAlt3Ge,

                        ImageAlt4Ar = Product.ImageAlt4Ar,
                        ImageAlt4En = Product.ImageAlt4En,
                        ImageAlt4Ge = Product.ImageAlt4Ge,

                    };
                }
            }
            else
                AddEditProductSeoModel.Id = 0;
        }





        private async Task SaveAsync()
        {
           
                AddEditProductSeoModel.ProductId = ProductId;
                var response2 = await ProductSeoManager.SaveAsync(AddEditProductSeoModel);
                if (response2.Succeeded)
                {
                    _snackBar.Add(response2.Messages[0], Severity.Success);
                     Back();
                }
                else
                {
                    foreach (var message in response2.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            

        }
     
    }
}
