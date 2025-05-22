using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Application.Requests;
using SchoolV01.Client.Extensions;

using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;

using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Shared.Constants;
using SchoolV01.Client.Pages.ProductCategories;

namespace SchoolV01.Client.Pages.Products
{
    public partial class AddEditCompanyProduct
    {
        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
   

        [Parameter] public int ParentCategoryId { get; set; } = 0;
        [Parameter] public int SubCategoryId { get; set; } = 0;
        [Parameter] public int SubSubCategoryId { get; set; } = 0;
        [Parameter] public int SubSubSubCategoryId { get; set; } = 0;
        [Parameter] public int? DefaultCategoryId { get; set; } = 0;

        [Parameter] public int ProductId { get; set; } = 0;
        private string imageUrlForPreview1 { get; set; } = "";
        private string imageUrlForPreview2 { get; set; } = "";
        private string imageUrlForPreview3 { get; set; } = "";
        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton1 { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        private AddEditCompanyProductCommand AddEditCompanyProductModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;


        private bool _isProcessing = false;
        List<string> MenuPlans = new List<string> {
            "A" ,
            "B" ,
            "C" ,
            "D" ,
        };
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private List<GetAllProductCategoriesResponse> _allCategories = new();
        private List<GetAllProductCategoriesResponse> _parentCategories = new();
        private List<GetAllProductCategoriesResponse> _subCategories = new();
        private List<GetAllProductCategoriesResponse> _subSubCategories = new();
        private List<GetAllProductCategoriesResponse> _subSubSubCategories = new();


        private TextEditorConfig editorDescriptionAr1 = new TextEditorConfig("#editorAr1");
        private TextEditorConfig editorDescriptionAr2 = new TextEditorConfig("#editorAr2");
        private TextEditorConfig editorDescriptionAr3 = new TextEditorConfig("#editorAr3");
        private TextEditorConfig editorDescriptionAr4 = new TextEditorConfig("#editorAr4");

        private TextEditorConfig editorDescriptionEn1 = new TextEditorConfig("#editorEn1");
        private TextEditorConfig editorDescriptionEn2 = new TextEditorConfig("#editorEn2");
        private TextEditorConfig editorDescriptionEn3 = new TextEditorConfig("#editorEn3");
        private TextEditorConfig editorDescriptionEn4 = new TextEditorConfig("#editorEn4");


        private TextEditorConfig editorDescriptionGe1 = new TextEditorConfig("#editorGe1");
        private TextEditorConfig editorDescriptionGe2 = new TextEditorConfig("#editorGe2");
        private TextEditorConfig editorDescriptionGe3 = new TextEditorConfig("#editorGe3");
        private TextEditorConfig editorDescriptionGe4 = new TextEditorConfig("#editorGe4");



        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; }
        private bool DisableAddButton = true;
        private decimal OldPrice = 0;
        public static long maxFileSize = 10 * 1024 * 1024;
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }
        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
            //MudDialog.Cancel();

        }
        private async Task LoadDataAsync()
        {
            await Task.WhenAll(
             LoadProductParentCategories(),
             LoadProductDetails()
            );
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

                _subCategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId).ToList();
            }
            DefaultCategoryId = ParentCategoryId;
         

        }

        private async Task LoadProductSubCategorySons()
        {
            if (SubCategoryId == 0)
            {
                DefaultCategoryId = ParentCategoryId;
                return;
            }
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubCategories = data.Data.Where(x => x.ParentCategoryId == SubCategoryId).ToList();
            }
            DefaultCategoryId = SubCategoryId;

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
    


       
        private async Task LoadProductDetails()
        {
            if (ProductId != 0)
            {
                var data = await ProductManager.GetByIdAsync(ProductId);
                if (data.Succeeded)
                {
                    var product = data.Data;
                    AddEditCompanyProductModel = new AddEditCompanyProductCommand
                    {
                        Id = product.Id,
                        NameAr = product.NameAr,
                        NameEn = product.NameEn,
                        NameGe = product.NameGe,

                        DescriptionAr1 = product.DescriptionAr1,
                        DescriptionAr2 = product.DescriptionAr2,
                        DescriptionAr3 = product.DescriptionAr3,
                        DescriptionAr4 = product.DescriptionAr4,
              

                        DescriptionEn1 = product.DescriptionEn1,
                        DescriptionEn2 = product.DescriptionEn2,
                        DescriptionEn3 = product.DescriptionEn3,
                        DescriptionEn4 = product.DescriptionEn4,


                        DescriptionGe1 = product.DescriptionGe1,
                        DescriptionGe2 = product.DescriptionGe2,
                        DescriptionGe3 = product.DescriptionGe3,
                        DescriptionGe4 = product.DescriptionGe4,
                       

                       
                        Code = product.Code,
                       

                        ProductParentCategoryId = product.ProductParentCategoryId,
                        ProductSubCategoryId = product.ProductSubCategoryId,
                        ProductSubSubCategoryId = product.ProductSubSubCategoryId,
                        ProductSubSubSubCategoryId = product.ProductSubSubSubCategoryId,
                        ProductDefaultCategoryId = product.ProductDefaultCategoryId,
             
                        Price = product.Price,
                    
                        IsVisible = product.IsVisible,
                        IsRecent = product.IsRecent,
                        Order = product.Order,
                 
                      
                    };
                    if (!String.IsNullOrEmpty(AddEditCompanyProductModel.ProductImageUrl1))
                    {
                        imageUrlForPreview1 = AddEditCompanyProductModel.ProductImageUrl1;
                        disableDeleteImageButton1 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditCompanyProductModel.ProductImageUrl2))
                    {
                        imageUrlForPreview2 = AddEditCompanyProductModel.ProductImageUrl2;
                        disableDeleteImageButton2 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditCompanyProductModel.ProductImageUrl3))
                    {
                        imageUrlForPreview3 = AddEditCompanyProductModel.ProductImageUrl3;
                        disableDeleteImageButton3 = false;
                    }

                    DisableAddButton = false;
                    OldPrice = product.Price.Value;
                    if (product.ProductParentCategoryId is not null)
                    {
                        ParentCategoryId = product.ProductParentCategoryId.Value;
                        await LoadProductParentCategorySons();
                    }

                    if (product.ProductSubCategoryId != null)
                    {

                        SubCategoryId = product.ProductSubCategoryId.Value;

                        await LoadProductSubCategorySons();

                    }
                    if (product.ProductSubSubCategoryId != null)
                    {
                        SubSubCategoryId = product.ProductSubSubCategoryId.Value;
                        await LoadProductSubSubCategorySons();
                    }

                    if (product.ProductSubSubSubCategoryId != null)
                        SubSubSubCategoryId = product.ProductSubSubSubCategoryId.Value;
                    DefaultCategoryId = product.ProductDefaultCategoryId;
                }
            }
            else
            {
                AddEditCompanyProductModel.Id = 0;
            }

        }

      
       
    
        private async Task SaveAsync()
        {
            _isProcessing = true;
            if (!AddEditCompanyProductModel.Price.HasValue)
                AddEditCompanyProductModel.Price = 0;
            
            if (SubCategoryId == 0)
            {
                AddEditCompanyProductModel.ProductDefaultCategoryId = ParentCategoryId;
                AddEditCompanyProductModel.ProductParentCategoryId = ParentCategoryId;
            }
            else if (SubSubCategoryId == 0)
            {
                AddEditCompanyProductModel.ProductParentCategoryId = ParentCategoryId;
                AddEditCompanyProductModel.ProductSubCategoryId = SubCategoryId;
                AddEditCompanyProductModel.ProductDefaultCategoryId = SubCategoryId;
                AddEditCompanyProductModel.ProductSubSubCategoryId = 0;
                AddEditCompanyProductModel.ProductSubSubSubCategoryId = 0;
            }
            else if (SubSubSubCategoryId == 0)
            {
                AddEditCompanyProductModel.ProductParentCategoryId = ParentCategoryId;
                AddEditCompanyProductModel.ProductSubCategoryId = SubCategoryId;
                AddEditCompanyProductModel.ProductDefaultCategoryId = SubSubCategoryId;
                AddEditCompanyProductModel.ProductSubSubCategoryId = SubSubCategoryId;
                AddEditCompanyProductModel.ProductSubSubSubCategoryId = 0;
            }
            else
            {
                AddEditCompanyProductModel.ProductParentCategoryId = ParentCategoryId;
                AddEditCompanyProductModel.ProductSubCategoryId = SubCategoryId;
                AddEditCompanyProductModel.ProductDefaultCategoryId = SubSubSubCategoryId;
                AddEditCompanyProductModel.ProductSubSubCategoryId = SubSubCategoryId;
                AddEditCompanyProductModel.ProductSubSubSubCategoryId = SubSubSubCategoryId;
            }
       
     

            var response = await ProductManager.SaveForCompanyProfileAsync(AddEditCompanyProductModel);
            if (response.Succeeded)
            {
                AddEditCompanyProductModel.Id = response.Data;
                ProductId = response.Data;
                DisableAddButton = false;
                OldPrice = AddEditCompanyProductModel.Price.Value;
                _snackBar.Add(response.Messages[0], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            _isProcessing = false;
        }

        private void DeleteURL1Async()
        {
            AddEditCompanyProductModel.ProductImageUrl1 = null;
            disableDeleteImageButton1 = true;
            imageUrlForPreview1 = null;

            AddEditCompanyProductModel.UploadRequestURL1 = new UploadRequest();
        }
        private void DeleteURL2Async()
        {
            AddEditCompanyProductModel.ProductImageUrl2 = null;
            disableDeleteImageButton2 = true;
            imageUrlForPreview2 = null;

            AddEditCompanyProductModel.UploadRequestURL2 = new UploadRequest();
        }
        private void DeleteURL3Async()
        {
            AddEditCompanyProductModel.ProductImageUrl3 = null;
            disableDeleteImageButton1 = true;
            imageUrlForPreview3 = null;
            AddEditCompanyProductModel.UploadRequestURL3 = new UploadRequest();
        }

        private IBrowserFile _imageFile;
        private IBrowserFile _imageFile2;
        private IBrowserFile _imageFile3;


        private async Task UploadFiles(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                //var buffer = new byte[imageFile.Size];
                //await imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var buffer = new byte[_imageFile.Size];
                await _imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl1 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequestURL1 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
            }
        }


        private async Task UploadFiles2(InputFileChangeEventArgs e)
        {
            _imageFile2 = e.File;
            if (_imageFile2 != null)
            {
                var extension = Path.GetExtension(_imageFile2.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                //var buffer = new byte[imageFile.Size];
                //await imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var buffer = new byte[_imageFile.Size];
                await _imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequestURL2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
            }
        }


        private async Task UploadFiles3(InputFileChangeEventArgs e)
        {
            _imageFile3 = e.File;
            if (_imageFile3 != null)
            {
                var extension = Path.GetExtension(_imageFile3.Name);
                var format = "image/png";
                //var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                //var buffer = new byte[imageFile.Size];
                //await imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                var buffer = new byte[_imageFile.Size];
                await _imageFile.OpenReadStream(maxFileSize).ReadAsync(buffer);
                AddEditCompanyProductModel.ProductImageUrl3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyProductModel.UploadRequestURL3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
            }
        }


    }
}
