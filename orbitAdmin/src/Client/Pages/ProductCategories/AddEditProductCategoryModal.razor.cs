
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Application.Features.ProductCategories.Commands.AddEdit;
using SchoolV01.Application.Features.ProductCategories.Queries.GetAll;
using SchoolV01.Application.Requests;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.ProductCategory;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Client.Infrastructure.Managers.Products;
using System.Threading;
using Azure;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.ProductCategories
{
    public partial class AddEditProductCategoryModal
    {

        [Parameter]
        public int Id { get; set; }
        [Inject] private IProductCategoryManager ProductCategoryManager { get; set; }
        private string imageUrlForPreview1 { get; set; } = "";
        private string imageUrlForPreview2 { get; set; } = "";
        private string imageUrlForPreview3 { get; set; } = "";
        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton1 { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        [Parameter] public AddEditProductCategoryCommand AddEditProductCategoryModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllProductCategoriesResponse> _ProductCategories = new();
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



        private bool _isProcessing = false;
        public static long maxFileSize = 1024 * 1024;
        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }

        private async Task SaveAsync()
        {
            _isProcessing = true;
            var response = await ProductCategoryManager.SaveAsync(AddEditProductCategoryModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                MudDialog.Close();
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

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
   
            await LoadProductCategoriesAsync();
            await LoadProductCategoriesDetails();


        }

        private async Task LoadProductCategoriesAsync()
        {
            _ProductCategories.Clear();
            var data = await ProductCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _ProductCategories = data.Data;

            }
        }
        private async Task LoadProductCategoriesDetails()
        {
            if (Id != 0)
            {
                var data = await ProductCategoryManager.GetByIdAsync(Id);
                if (data.Succeeded)
                {
                    var product = data.Data;
                    AddEditProductCategoryModel = new AddEditProductCategoryCommand
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

                         ImageDataURL1 = product.ImageDataURL1,
                         ImageDataURL2 = product.ImageDataURL2,
                         ImageDataURL3 = product.ImageDataURL3,
                        Order = product.Order,
                       ParentCategoryId = product.ParentCategoryId,
                    };

                    if (!String.IsNullOrEmpty(AddEditProductCategoryModel.ImageDataURL1))
                    {
                        imageUrlForPreview1 = AddEditProductCategoryModel.ImageDataURL1;
                        disableDeleteImageButton1 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditProductCategoryModel.ImageDataURL2))
                    {
                        imageUrlForPreview2 = AddEditProductCategoryModel.ImageDataURL2;
                        disableDeleteImageButton2 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditProductCategoryModel.ImageDataURL3))
                    {
                        imageUrlForPreview3 = AddEditProductCategoryModel.ImageDataURL3;
                        disableDeleteImageButton3 = false;
                    }
                }
            }
            else
            {
                AddEditProductCategoryModel.Id = 0;
            }

        }




        private void DeleteURL1Async()
        {
            AddEditProductCategoryModel.ImageDataURL1 = null;
            AddEditProductCategoryModel.UploadRequestURL1 = new UploadRequest();
            disableDeleteImageButton1 = true;
            imageUrlForPreview1 = null;

        }
        private void DeleteURL2Async()
        {
            AddEditProductCategoryModel.ImageDataURL2 = null;
            AddEditProductCategoryModel.UploadRequestURL2 = new UploadRequest();
            disableDeleteImageButton2 = true;
            imageUrlForPreview2 = null;

        }
        private void DeleteURL3Async()
        {
            AddEditProductCategoryModel.ImageDataURL3 = null;
            disableDeleteImageButton3 = true;
            imageUrlForPreview3 = null;

            AddEditProductCategoryModel.UploadRequestURL3 = new UploadRequest();
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
                AddEditProductCategoryModel.ImageDataURL1 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductCategoryModel.UploadRequestURL1 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
                
                
                imageUrlForPreview1 = AddEditProductCategoryModel.ImageDataURL1;
                disableDeleteImageButton1 = false;

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
                AddEditProductCategoryModel.ImageDataURL2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductCategoryModel.UploadRequestURL2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
                imageUrlForPreview2 = AddEditProductCategoryModel.ImageDataURL2;
                disableDeleteImageButton2 = false;
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
                AddEditProductCategoryModel.ImageDataURL3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditProductCategoryModel.UploadRequestURL3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };

                imageUrlForPreview3 = AddEditProductCategoryModel.ImageDataURL3;
                disableDeleteImageButton3 = false;
            }
        }


        private async Task<IEnumerable<int?>> SearchCategories(string value,CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _ProductCategories.Select(x => x?.Id);

            return _ProductCategories.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x?.Id).ToList();
        }
    }
}
