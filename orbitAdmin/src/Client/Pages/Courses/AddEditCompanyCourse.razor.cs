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

using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Requests;
using SchoolV01.Client.Extensions;

using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory;

using SchoolV01.Client.Infrastructure.Managers.Courses;
using SchoolV01.Shared.Constants;
using SchoolV01.Client.Pages.CourseCategories;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Application.Features.CourseTypes.Queries;
using System.Globalization;
using SchoolV01.Domain.Entities.Products;

namespace SchoolV01.Client.Pages.Courses
{
    public partial class AddEditCompanyCourse
    {
        [Inject] private ICourseManager CourseManager { get; set; }
        [Inject] private ICourseTypeManager CourseTypeManager { get; set; }
        [Inject] private ICourseCategoryManager CourseCategoryManager { get; set; }

        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Parameter] public int ParentCategoryId { get; set; } = 0;
        [Parameter] public int SubCategoryId { get; set; } = 0;
        [Parameter] public int SubSubCategoryId { get; set; } = 0;
        [Parameter] public int SubSubSubCategoryId { get; set; } = 0;
        [Parameter] public int? DefaultCategoryId { get; set; } = 0;

        [Parameter] public int CourseId { get; set; } = 0;
        private string imageUrlForPreview1 { get; set; } = "";
        private string imageUrlForPreview2 { get; set; } = "";
        private string imageUrlForPreview3 { get; set; } = "";
        private string noImageUrl = Constants.NOImagePath;
        private bool disableDeleteImageButton1 { get; set; } = true;
        private bool disableDeleteImageButton2 { get; set; } = true;
        private bool disableDeleteImageButton3 { get; set; } = true;
        private AddEditCompanyCourseCommand AddEditCompanyCourseModel { get; set; } = new();

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

        private List<GetAllCourseCategoriesResponse> _allCategories = new();
        private List<GetAllCourseTypesResponse> _courseType = new();
        private List<GetAllCourseCategoriesResponse> _parentCategories = new();
        private List<GetAllCourseCategoriesResponse> _subCategories = new();
        private List<GetAllCourseCategoriesResponse> _subSubCategories = new();
        private List<GetAllCourseCategoriesResponse> _subSubSubCategories = new();


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
             LoadCourseParentCategories(),
             LoadCourseTypes(),
             LoadCourseDetails()
            );
        }


        private async Task LoadCourseTypes()
        {
            var data = await CourseTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _courseType = data.Data.ToList();
            }
        }

        private async Task LoadCourseParentCategories()
        {
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _parentCategories = data.Data.Where(x => (x.ParentCategoryId == null || x.ParentCategoryId == 0)).ToList();
            }
        }

        private async Task LoadCourseParentCategorySons()
        {

            if (ParentCategoryId == 0)
                return;
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {

                _subCategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId).ToList();
            }
            DefaultCategoryId = ParentCategoryId;
         

        }

        private async Task LoadCourseSubCategorySons()
        {
            if (SubCategoryId == 0)
            {
                DefaultCategoryId = ParentCategoryId;
                return;
            }
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubCategories = data.Data.Where(x => x.ParentCategoryId == SubCategoryId).ToList();
            }
            DefaultCategoryId = SubCategoryId;

        }
        private async Task LoadCourseSubSubCategorySons()
        {
            if (SubSubCategoryId == 0)
            {
                DefaultCategoryId = SubCategoryId;
                return;
            }
            //_subSubSubCategories.Clear();
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubSubCategories = data.Data.Where(x => x.ParentCategoryId == SubSubCategoryId).ToList();
            }
            DefaultCategoryId = SubSubCategoryId;
            //_sizes.Clear();

            //await LoadSizes(SubSubCategoryId);
            //await LoadCourseSizeColors(CourseId, SubSubCategoryId);

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
    


       
        private async Task LoadCourseDetails()
        {
            if (CourseId != 0)
            {
                var data = await CourseManager.GetByIdAsync(CourseId);
                if (data.Succeeded)
                {
                    var Course = data.Data;
                    AddEditCompanyCourseModel = new AddEditCompanyCourseCommand
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
                        Keywords = Course.Keywords,
                         SeoDescription = Course.SeoDescription,
                          Plan = Course.Plan,

                       
                        Code = Course.Code,
                        CourseTypeId = Course.CourseTypeId,
                        NumSesstions = Course.NumSesstions,
                        StartDate = Course.StartDate,
                        TeacherNameEn = Course.TeacherNameEn,
                        TeacherNameAr = Course.TeacherNameAr,
                        TeacherNameGe = Course.TeacherNameGe,
                        StartEnd = Course.StartEnd,
                        NumMaxStudent = Course.NumMaxStudent,

                        CourseParentCategoryId = Course.CourseParentCategoryId,
                        CourseSubCategoryId = Course.CourseSubCategoryId,
                        CourseSubSubCategoryId = Course.CourseSubSubCategoryId,
                        CourseSubSubSubCategoryId = Course.CourseSubSubSubCategoryId,
                        CourseDefaultCategoryId = Course.CourseDefaultCategoryId,
                   
                        Price = Course.Price,
                    
                        IsVisible = Course.IsVisible,
                        IsRecent = Course.IsRecent,
                        Order = Course.Order,
                      CourseImageUrl1 = Course.CourseImageUrl1,
                      CourseImageUrl2 = Course.CourseImageUrl2,
                      CourseImageUrl3 = Course.CourseImageUrl3,
                      
                    };
                    if (!string.IsNullOrEmpty(Course.CourseImageUrl1))
                    {
                        disableDeleteImageButton1 = false;

                    }
                    if (!string.IsNullOrEmpty(Course.CourseImageUrl2))
                    {
                        disableDeleteImageButton2 = false;

                    }
                    if (!string.IsNullOrEmpty(Course.CourseImageUrl3))
                    {
                        disableDeleteImageButton3 = false;

                    }

                    DisableAddButton = false;
                    OldPrice = Course.Price.Value;
                    if (Course.CourseParentCategoryId is not null)
                    {
                        ParentCategoryId = Course.CourseParentCategoryId.Value;
                        await LoadCourseParentCategorySons();
                    }

                    if (Course.CourseSubCategoryId != null)
                    {

                        SubCategoryId = Course.CourseSubCategoryId.Value;

                        await LoadCourseSubCategorySons();

                    }
                    if (Course.CourseSubSubCategoryId != null)
                    {
                        SubSubCategoryId = Course.CourseSubSubCategoryId.Value;
                        await LoadCourseSubSubCategorySons();
                    }

                    if (Course.CourseSubSubSubCategoryId != null)
                        SubSubSubCategoryId = Course.CourseSubSubSubCategoryId.Value;
                    DefaultCategoryId = Course.CourseDefaultCategoryId;
                }
            }
            else
            {
                AddEditCompanyCourseModel.Id = 0;
            }

        }

      
       
    
        private async Task SaveAsync()
        {
            _isProcessing = true;
            if (!AddEditCompanyCourseModel.Price.HasValue)
                AddEditCompanyCourseModel.Price = 0;
            
            if (SubCategoryId == 0)
            {
                AddEditCompanyCourseModel.CourseDefaultCategoryId = ParentCategoryId;
                AddEditCompanyCourseModel.CourseParentCategoryId = ParentCategoryId;
            }
            else if (SubSubCategoryId == 0)
            {
                AddEditCompanyCourseModel.CourseParentCategoryId = ParentCategoryId;
                AddEditCompanyCourseModel.CourseSubCategoryId = SubCategoryId;
                AddEditCompanyCourseModel.CourseDefaultCategoryId = SubCategoryId;
                AddEditCompanyCourseModel.CourseSubSubCategoryId = 0;
                AddEditCompanyCourseModel.CourseSubSubSubCategoryId = 0;
            }
            else if (SubSubSubCategoryId == 0)
            {
                AddEditCompanyCourseModel.CourseParentCategoryId = ParentCategoryId;
                AddEditCompanyCourseModel.CourseSubCategoryId = SubCategoryId;
                AddEditCompanyCourseModel.CourseDefaultCategoryId = SubSubCategoryId;
                AddEditCompanyCourseModel.CourseSubSubCategoryId = SubSubCategoryId;
                AddEditCompanyCourseModel.CourseSubSubSubCategoryId = 0;
            }
            else
            {
                AddEditCompanyCourseModel.CourseParentCategoryId = ParentCategoryId;
                AddEditCompanyCourseModel.CourseSubCategoryId = SubCategoryId;
                AddEditCompanyCourseModel.CourseDefaultCategoryId = SubSubSubCategoryId;
                AddEditCompanyCourseModel.CourseSubSubCategoryId = SubSubCategoryId;
                AddEditCompanyCourseModel.CourseSubSubSubCategoryId = SubSubSubCategoryId;
            }
       
     

            var response = await CourseManager.SaveForCompanyProfileAsync(AddEditCompanyCourseModel);
            if (response.Succeeded)
            {
                AddEditCompanyCourseModel.Id = response.Data;
                CourseId = response.Data;
                DisableAddButton = false;
                OldPrice = AddEditCompanyCourseModel.Price.Value;
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
            AddEditCompanyCourseModel.CourseImageUrl1 = null;
            disableDeleteImageButton1 = true;
            imageUrlForPreview1 = null;

            AddEditCompanyCourseModel.UploadRequestURL1 = new UploadRequest();
        }
        private void DeleteURL2Async()
        {
            AddEditCompanyCourseModel.CourseImageUrl2 = null;
            disableDeleteImageButton2 = true;
            imageUrlForPreview2 = null;

            AddEditCompanyCourseModel.UploadRequestURL2 = new UploadRequest();
        }
        private void DeleteURL3Async()
        {
            AddEditCompanyCourseModel.CourseImageUrl3 = null;
            disableDeleteImageButton3 = true;
            imageUrlForPreview3 = null;
            AddEditCompanyCourseModel.UploadRequestURL3 = new UploadRequest();
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
                AddEditCompanyCourseModel.CourseImageUrl1 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyCourseModel.UploadRequestURL1 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };

                imageUrlForPreview1 = AddEditCompanyCourseModel.CourseImageUrl1;
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
                AddEditCompanyCourseModel.CourseImageUrl2 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyCourseModel.UploadRequestURL2 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };


                imageUrlForPreview2 = AddEditCompanyCourseModel.CourseImageUrl2;
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
                AddEditCompanyCourseModel.CourseImageUrl3 = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyCourseModel.UploadRequestURL3 = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.ProductCategory, Extension = extension };


                imageUrlForPreview3 = AddEditCompanyCourseModel.CourseImageUrl3;
                disableDeleteImageButton3 = false;
            }
        }


    }
}
