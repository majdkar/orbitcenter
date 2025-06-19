using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using SchoolV01.Shared.Constants.Application;

namespace SchoolV01.Client.Pages.Courses
{
    public partial class AddEditCourseOfferModal
    {
        [Inject] private ICourseOfferManager CourseOfferManager { get; set; }

        [Inject] private ICourseCategoryManager CourseCategoryManager { get; set; }

        [Parameter] public AddEditCourseOfferCommand AddEditCourseOfferModel { get; set; } = new();
        [Parameter] public AddEditCompanyCourseCommand AddEditCompanyCourseModel { get; set; } = new();

        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
         public int ParentCategoryId { get; set; } = 0;
         public int SubCategoryId { get; set; } = 0;
         public int SubSubCategoryId { get; set; } = 0;
         public int SubSubSubCategoryId { get; set; } = 0;
         public int DefaultCategoryId { get; set; } = 0;

        public int CourseId { get; set; } = 0;
        private List<GetAllCourseCategoriesResponse> _allCategories = new();
        private List<GetAllCourseCategoriesResponse> _parentCategories = new();
        private List<GetAllCourseCategoriesResponse> _subCategories = new();
        private List<GetAllCourseCategoriesResponse> _subSubCategories = new();
        private List<GetAllCourseCategoriesResponse> _subSubSubCategories = new();
        private bool _isProcessing = false;
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            _isProcessing = true;
            var response = await CourseOfferManager.SaveAsync(AddEditCourseOfferModel);
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

            ParentCategoryId = AddEditCompanyCourseModel.CourseParentCategoryId ?? 0;
            SubCategoryId = AddEditCompanyCourseModel.CourseSubCategoryId ?? 0;
            SubSubCategoryId = AddEditCompanyCourseModel.CourseSubSubCategoryId ?? 0;
            SubSubSubCategoryId = AddEditCompanyCourseModel.CourseSubSubSubCategoryId ?? 0;
            await LoadCourseParentCategories();
           await LoadCourseParentCategorySons();
           await LoadCourseSubCategorySons();
           await LoadCourseSubSubCategorySons();
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

                _subCategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId ).ToList();
            }
            DefaultCategoryId = ParentCategoryId;
            //_sizes.Clear();

            //await LoadSizes(ParentCategoryId);
            //await LoadCourseSizeColors(CourseId, ParentCategoryId);


        }

        private async Task LoadCourseSubCategorySons()
        {
            if (SubCategoryId == 0)
            {
                DefaultCategoryId = ParentCategoryId;
                return;
            }
            //_subSubCategories.Clear();
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _subSubCategories = data.Data.Where(x => x.ParentCategoryId == SubCategoryId ).ToList();
            }
            DefaultCategoryId = SubCategoryId;
            //_sizes.Clear();

            //await LoadSizes(SubCategoryId);
            //await LoadCourseSizeColors(CourseId, SubCategoryId);
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

        private void CalculateNewPrice()
        {
            AddEditCourseOfferModel.NewPrice = Math.Round((decimal)(AddEditCourseOfferModel.OldPrice - (AddEditCourseOfferModel.OldPrice * AddEditCourseOfferModel.DiscountRatio / 100)), 2);
        }

     


    }
}

