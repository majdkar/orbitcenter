
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
using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Requests;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using System.Threading;
using Azure;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.CourseCategories
{
    public partial class AddEditCourseCategoryModal
    {

        [Parameter]
        public int Id { get; set; }      
        
        [Parameter]
        public int CategoryId { get; set; }
        [Inject] private ICourseCategoryManager CourseCategoryManager { get; set; }
        private string imageUrlForPreview1 { get; set; } = "";
        private string imageUrlForPreview2 { get; set; } = "";
        private string imageUrlForPreview3 { get; set; } = "";
        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton1 { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        [Parameter] public AddEditCourseCategoryCommand AddEditCourseCategoryModel { get; set; } = new();
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private List<GetAllCourseCategoriesResponse> _CourseCategories = new();
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
        public static long maxFileSize = 1024 * 1024 *1024;
        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }

        private async Task SaveAsync()
        {
            _isProcessing = true;
            var response = await CourseCategoryManager.SaveAsync(AddEditCourseCategoryModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                Cancel();
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
   
            await LoadCourseCategoriesAsync();
            await LoadCourseCategoriesDetails();


        }

        private async Task LoadCourseCategoriesAsync()
        {
            if (CategoryId == 0)
            {
                _CourseCategories.Clear();
                var data = await CourseCategoryManager.GetAllAsync();
                if (data.Succeeded)
                {
                    _CourseCategories = data.Data;

                }
            }
            else
            {
                _CourseCategories.Clear();
                var data = await CourseCategoryManager.GetAllAsync();
                if (data.Succeeded)
                {
                    _CourseCategories = data.Data;
                    AddEditCourseCategoryModel.ParentCategoryId = CategoryId;

                }
            }
        }
        private async Task LoadCourseCategoriesDetails()
        {
            if (Id != 0)
            {
                var data = await CourseCategoryManager.GetByIdAsync(Id);
                if (data.Succeeded)
                {
                    var Course = data.Data;
                    AddEditCourseCategoryModel = new AddEditCourseCategoryCommand
                    {
                        Id = Course.Id,
                        NameAr = Course.NameAr,
                        NameEn = Course.NameEn,
                        NameGe = Course.NameGe,

                        DescriptionAr1 = Course.DescriptionAr1,
                        DescriptionAr2 = Course.DescriptionAr2,
                        DescriptionAr3 = Course.DescriptionAr3,
                        DescriptionAr4 = Course.DescriptionAr4,

                        DescriptionEn1 = Course.DescriptionEn1,
                        DescriptionEn2 = Course.DescriptionEn2,
                        DescriptionEn3 = Course.DescriptionEn3,
                        DescriptionEn4 = Course.DescriptionEn4,

                        DescriptionGe1 = Course.DescriptionGe1,
                        DescriptionGe2 = Course.DescriptionGe2,
                        DescriptionGe3 = Course.DescriptionGe3,
                        DescriptionGe4 = Course.DescriptionGe4,

                         ImageDataURL1 = Course.ImageDataURL1,
                         ImageDataURL2 = Course.ImageDataURL2,
                         ImageDataURL3 = Course.ImageDataURL3,
                        Order = Course.Order,
                       ParentCategoryId = Course.ParentCategoryId,
                    };

                    if (!String.IsNullOrEmpty(AddEditCourseCategoryModel.ImageDataURL1))
                    {
                        imageUrlForPreview1 = AddEditCourseCategoryModel.ImageDataURL1;
                        disableDeleteImageButton1 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditCourseCategoryModel.ImageDataURL2))
                    {
                        imageUrlForPreview2 = AddEditCourseCategoryModel.ImageDataURL2;
                        disableDeleteImageButton2 = false;
                    }
                    if (!String.IsNullOrEmpty(AddEditCourseCategoryModel.ImageDataURL3))
                    {
                        imageUrlForPreview3 = AddEditCourseCategoryModel.ImageDataURL3;
                        disableDeleteImageButton3 = false;
                    }
                }
            }
            else
            {
                AddEditCourseCategoryModel.Id = 0;
            }

        }




        private void DeleteURL1Async()
        {
            AddEditCourseCategoryModel.ImageDataURL1 = null;
            AddEditCourseCategoryModel.UploadRequestURL1 = new UploadRequest();
            disableDeleteImageButton1 = true;
            imageUrlForPreview1 = null;

        }
        private void DeleteURL2Async()
        {
            AddEditCourseCategoryModel.ImageDataURL2 = null;
            AddEditCourseCategoryModel.UploadRequestURL2 = new UploadRequest();
            disableDeleteImageButton2 = true;
            imageUrlForPreview2 = null;

        }
        private void DeleteURL3Async()
        {
            AddEditCourseCategoryModel.ImageDataURL3 = null;
            disableDeleteImageButton3 = true;
            imageUrlForPreview3 = null;

            AddEditCourseCategoryModel.UploadRequestURL3 = new UploadRequest();
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
                AddEditCourseCategoryModel.ImageDataURL1 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCourseCategoryModel.UploadRequestURL1 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
                
                
                imageUrlForPreview1 = AddEditCourseCategoryModel.ImageDataURL1;
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
                var buffer = new byte[_imageFile2.Size];
                await _imageFile2.OpenReadStream(maxFileSize).ReadAsync(buffer);
                AddEditCourseCategoryModel.ImageDataURL2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCourseCategoryModel.UploadRequestURL2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };
                imageUrlForPreview2 = AddEditCourseCategoryModel.ImageDataURL2;
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
                var buffer = new byte[_imageFile3.Size];
                await _imageFile3.OpenReadStream(maxFileSize).ReadAsync(buffer);
                AddEditCourseCategoryModel.ImageDataURL3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCourseCategoryModel.UploadRequestURL3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };

                imageUrlForPreview3 = AddEditCourseCategoryModel.ImageDataURL3;
                disableDeleteImageButton3 = false;
            }
        }


        private async Task<IEnumerable<int?>> SearchCategories(string value,CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _CourseCategories.Select(x => x?.Id);

            return _CourseCategories.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x?.Id).ToList();
        }
    }
}
