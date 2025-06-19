using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using SchoolV01.Application.Features.CourseCategories.Commands.AddEdit;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAll;
using SchoolV01.Application.Features.CourseCategories.Queries.GetAllPaged;
using SchoolV01.Application.Requests.CourseCategories;
using SchoolV01.Application.Requests.Courses;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.CourseCategory;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolV01.Client.Pages.CourseCategories
{
    public partial class CourseCategories
    {
        [Inject] private ICourseCategoryManager CourseCategoryManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        //private List<GetAllCourseCategoriesResponse> _CategoriesList = new();

        private GetAllCourseCategoriesResponse _Category  = new();
        private IEnumerable<GetAllPagedCourseCategoriesResponse> _pagedData;
        private MudTable<GetAllPagedCourseCategoriesResponse> _table;

        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;
        [Parameter] public int ParentCategoryId { get; set; } = 0;
        [Parameter] public int SubCategoryId { get; set; } = 0;
        [Parameter] public int CategoryId { get; set; } = 0;
        private List<GetAllCourseCategoriesResponse> _parentCategories = new();
        private List<GetAllCourseCategoriesResponse> _subCategories = new();
        private List<GetAllCourseCategoriesResponse> _allCategories = new();
        private ClaimsPrincipal _currentUser;
        private bool _canCreateCourseCategories;
        private bool _canEditCourseCategories;
        private bool _canDeleteCourseCategories;
        private bool _canExportCourseCategories;
        private bool _canSearchCourseCategories;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCourseCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.CourseCategories.Create)).Succeeded;
            _canEditCourseCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.CourseCategories.Edit)).Succeeded;
            _canDeleteCourseCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.CourseCategories.Delete)).Succeeded;
            _canExportCourseCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.CourseCategories.View)).Succeeded;
            _canSearchCourseCategories = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.CourseCategories.View)).Succeeded;
           
            //_navigationManager.TryGetQueryString<string>("CategoryId", out CategoryId);
          
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
                
            }
            await LoadAllCourseCategories();
            //await LoadCourseParentCategories();
           // _snackBar.Add(CategoryId.ToString(), Severity.Error);
        }
        private async Task LoadCourseSubCategorySons()
        {
            //await LoadAllCourseCategories();
            await _table.ReloadServerData();
        }
        private async Task SearchAdvance()
        {
            //await LoadAllCourseCategories();
            //await LoadCourseParentCategories();
            await _table.ReloadServerData();
        }
        private async Task<TableData<GetAllPagedCourseCategoriesResponse>> ServerReload(TableState state,CancellationToken token)
        {
            if (!string.IsNullOrWhiteSpace(_searchString))
            {
                state.Page = 0;
            }
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedCourseCategoriesResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadAllCourseCategories()
        {
            //_allCategories.Clear();
            var data = await CourseCategoryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _allCategories = data.Data;
                _parentCategories = _allCategories.Where(x => (x.ParentCategoryId == null) || (x.ParentCategoryId == 0)).ToList();

            }
        }
        //private async Task LoadCourseParentCategories()
        //{
        //    //_parentCategories.Clear();
        //    var data = await CourseCategoryManager.GetAllAsync();
        //    if (data.Succeeded)
        //    {
        //        _parentCategories = data.Data.Where(x => (x.ParentCategoryId == null) || (x.ParentCategoryId == 0)).ToList();
        //    }
        //}

        private void LoadCourseParentCategorySons()
        {

            if (ParentCategoryId == 0)
                return;
            _subCategories.Clear();
            _subCategories = _allCategories.Where(x => x.ParentCategoryId == ParentCategoryId).ToList();


        }
        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            string[] orderings = null;
            if (!string.IsNullOrEmpty(state.SortLabel))
            {
                orderings = state.SortDirection != SortDirection.None ? new[] { $"{state.SortLabel} {state.SortDirection}" } : new[] { $"{state.SortLabel}" };
            }

            var request = new GetAllPagedCourseCategoriesRequest { PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, Orderby = orderings };
            var response = await CourseCategoryManager.GetAllCategorySonsAsync(request,CategoryId);
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

        private async void InvokeBackModal(int id)
        {
            CategoryId = 0;
            _searchString = string.Empty;
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
        }

        private async void InvokeSons(int id)
        {
            CategoryId = id;
            _searchString = string.Empty;
            StateHasChanged();
            if (_table != null)
                await _table.ReloadServerData();
        }
        private async Task OnSearch(string text)
        {
            _searchString = text;
            await LoadAllCourseCategories();
            //await LoadCourseParentCategories();
            await _table.ReloadServerData();
        }
        private bool Search(GetAllCourseCategoriesResponse Category)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (Category.NameAr?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (Category.NameEn?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
           



            /**/
            return false;
        }

        private async Task ExportToExcel()
        {
            var response = await CourseCategoryManager.ExportToExcelAsync(_searchString);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{nameof(CourseCategories).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                    ? _localizer["CourseCategories exported"]
                    : _localizer["Filtered CourseCategories exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(int id = 0,int CategoryId = 0)
        {
            _navigationManager.NavigateTo($"CourseCategory-details/{id}/{CategoryId}");
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
                var response = await CourseCategoryManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                   await OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                  await  OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}
