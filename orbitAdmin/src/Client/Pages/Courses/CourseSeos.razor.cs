using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;

namespace SchoolV01.Client.Pages.Courses
{
    public partial  class CourseSeos
    {
        [Parameter] public int CourseId { get; set; }
        [Parameter] public string Name { get; set; }
        [Parameter] public bool DisableAddButton { get; set; } = false;
        [Parameter] public decimal OldPrice { get; set; } = 0;

        [Inject] private ICourseSeoManager CourseSeoManager { get; set; }
        [Parameter] public AddEditCompanyCourseCommand AddEditCompanyCourseModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCourseSeosResponse> _Seos = new();
        private GetAllCourseSeosResponse _Seo = new();

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCourseSeo;
        private bool _canEditCourseSeo;
        private bool _canDeleteCourseSeo;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCourseSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Create)).Succeeded;
            _canEditCourseSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Edit)).Succeeded;
            _canDeleteCourseSeo = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Delete)).Succeeded;

            await LoadSeosAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadSeosAsync()
        {
            if (CourseId != 0)
            {
                var response = await CourseSeoManager.GetAllByCourseAsync(CourseId);
                if (response.Succeeded)
                {
                    _Seos = response.Data.ToList();
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
                var response = await CourseSeoManager.DeleteAsync(id);
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
            _Seos = new List<GetAllCourseSeosResponse>();
            await LoadSeosAsync();
        }

        private async Task InvokeModal(int id = 0,int CourseId = 0)
        {
            _navigationManager.NavigateTo($"/seoCourse-details/{id}/{CourseId}");

        }

    }
}
