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

namespace SchoolV01.Client.Pages.Courses
{
    public partial  class CourseOffers
    {
        [Parameter] public int CourseId { get; set; }
        [Parameter] public bool DisableAddButton { get; set; } = true;
        [Parameter] public decimal OldPrice { get; set; } = 0;

        [Inject] private ICourseOfferManager CourseOfferManager { get; set; }
        [Parameter] public AddEditCompanyCourseCommand AddEditCompanyCourseModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<GetAllCourseOffersResponse> _offers = new();
        private GetAllCourseOffersResponse _offer = new();

        private ClaimsPrincipal _currentUser;
        private bool _canCreateCourseOffer;
        private bool _canEditCourseOffer;
        private bool _canDeleteCourseOffer;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateCourseOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Create)).Succeeded;
            _canEditCourseOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Edit)).Succeeded;
            _canDeleteCourseOffer = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Courses.Delete)).Succeeded;

            await LoadOffersAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task LoadOffersAsync()
        {
            if (CourseId != 0)
            {
                var response = await CourseOfferManager.GetAllByCourseAsync(CourseId);
                if (response.Succeeded)
                {
                    _offers = response.Data.ToList();
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
                var response = await CourseOfferManager.DeleteAsync(id);
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
            _offers = new List<GetAllCourseOffersResponse>();
            await LoadOffersAsync();
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0) // update
            {
                _offer = _offers.FirstOrDefault(c => c.Id == id);
                if (_offer != null)
                {
                    parameters.Add(nameof(AddEditCourseOfferModal.AddEditCourseOfferModel), new AddEditCourseOfferCommand
                    {
                        Id = _offer.Id,
                        CourseId = CourseId,
                        DiscountRatio = _offer.DiscountRatio,
                        NewPrice = _offer.NewPrice,
                        StartDate = _offer.StartDate,
                        EndDate = _offer.EndDate,
                        OldPrice = OldPrice,
                     
                    });
                    parameters.Add(nameof(AddEditCourseOfferModal.AddEditCompanyCourseModel), AddEditCompanyCourseModel);

                }
            }
            else // add
            {
                parameters.Add(nameof(AddEditCourseOfferModal.AddEditCourseOfferModel), new AddEditCourseOfferCommand
                {
                    Id = 0,
                    CourseId = CourseId,
                    OldPrice = OldPrice,
                    NewPrice = OldPrice,
                    DiscountRatio = 0
                    
                });

                parameters.Add(nameof(AddEditCourseOfferModal.AddEditCompanyCourseModel), AddEditCompanyCourseModel);
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = _dialogService.Show<AddEditCourseOfferModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await Reset();
            }
        }

    }
}
