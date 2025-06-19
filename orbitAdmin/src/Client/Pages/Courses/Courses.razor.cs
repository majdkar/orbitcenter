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
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using SchoolV01.Domain.Entities.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Threading;

namespace SchoolV01.Client.Pages.Courses
{
    public partial class Courses
    {
        [Inject] private ICourseManager CourseManager { get; set; }

        [Inject] private ICourseCategoryManager CourseCategoryManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        public string CourseName { get; set; }

        public int CourseCategoryId { get; set; }

        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public int SubSubCategoryId { get; set; }
        public int SubSubSubCategoryId { get; set; }


        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }

        private IEnumerable<GetAllPagedCoursesResponse> _pagedData;
        private MudTable<GetAllPagedCoursesResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCourse;
        private bool _canEditCourse;
        private bool _canDeleteCourse;
        private bool _canExportCourse;
        private bool _canSearchCourse;
        private bool _loaded;


        private IEnumerable<GetAllCourseCategoriesResponse> categories;
        private IEnumerable<GetAllCourseCategoriesResponse> Subcategories;
        private IEnumerable<GetAllCourseCategoriesResponse> SubSubcategories;
        private IEnumerable<GetAllCourseCategoriesResponse> SubSubSubcategories;
        
        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCourse = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Create)).Succeeded;
            _canEditCourse = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Edit)).Succeeded;
            _canDeleteCourse = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Delete)).Succeeded;
            _canExportCourse = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.View)).Succeeded;
            _canSearchCourse = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.View)).Succeeded;

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
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                categories = data.Data.Where(x => (x.ParentCategoryId == null || x.ParentCategoryId == 0));
            }
        }

  
        private async Task LoadsubCategories()
        {

            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                Subcategories = data.Data.Where(x => x.ParentCategoryId != null);
                SubSubcategories = data.Data.Where(x => x.ParentCategoryId != null);
                SubSubSubcategories = data.Data.Where(x => x.ParentCategoryId != null);
            }
        }
        private async Task LoadsubsubCategories()
        {

            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                SubSubcategories = data.Data.Where(x => x.ParentCategoryId == ParentCategoryId);
                await FilterData();
            }
        }    
        
        private async Task LoadsubsubsbCategories()
        {

            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                SubSubSubcategories = data.Data.Where(x => x.ParentCategoryId == SubSubCategoryId);
                await FilterData();
            }
        }

        private async Task LoadSubcategory()
        {
            var data = await CourseCategoryManager.GetAllAsync();
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

        private async Task<TableData<GetAllPagedCoursesResponse>> ServerReload(TableState state,CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedCoursesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedCoursesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };

            //if(SubSubSubCategoryId > 0)
            //{
            //    CourseCategoryId = SubSubSubCategoryId;
            //}
            //else if (SubSubCategoryId > 0)
            //{
            //    CourseCategoryId = SubSubCategoryId;

            //}
             if (ParentCategoryId > 0)
            {
                CourseCategoryId = ParentCategoryId;

            }
            else
            { CourseCategoryId = CategoryId;
            }

            var response = await CourseManager.GetAllPagedSearchCourseAsync(request, CourseName, CategoryId, ParentCategoryId, SubSubCategoryId, SubSubSubCategoryId, FromPrice
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
            CourseName = null;

            CourseCategoryId = 0;
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
            var response = await CourseManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(Course).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["Course exported"]
                    : _localizer["Filtered Course exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void RedirectToDetails(int CourseId)
        {
            _navigationManager.NavigateTo($"/Course-details/{CourseId}");
        }

        private string RedirectToViewDetails(int CourseId)
        {
            return $"/view-details/{CourseId}";
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
                var response = await CourseManager.DeleteAsync(id);
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
            _navigationManager.NavigateTo($"/CourseAlbumImage/{id}");
        }   private async Task InvokeDetailsModal(int id = 0)
        {
            _navigationManager.NavigateTo($"/CourseDetails/{id}");
        }
        private async Task InvokeModalOptions(int id = 0)
        {
            _navigationManager.NavigateTo($"/Course-options/{id}");
        }
        private async Task InvokeModalSizes(int id = 0)
        {
            _navigationManager.NavigateTo($"/Course-weights/{id}");
        }
        private async Task InvokeModalVideo(int id = 0)
        {
            _navigationManager.NavigateTo($"/CourseAlbumVideo/{id}");
        }

    }
}
