﻿using SchoolV01.Application.Responses.Identity;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using SchoolV01.Client.Pages.Communication;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Client.Pages.Identity
{
    public partial class Users
    {
        private List<UserResponse> _userList = new();
        private UserResponse _user = new();
        private string _searchString = "";
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateUsers;
        private bool _canSearchUsers;
        private bool _canExportUsers;
        private bool _canViewRoles;
        private bool _canEditUsers;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create)).Succeeded;
            _canEditUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Edit)).Succeeded;
            _canSearchUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.View)).Succeeded;
            _canExportUsers = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Users.View)).Succeeded;
            _canViewRoles = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View)).Succeeded;

            await GetUsersAsync();
            _loaded = true;
        }

        private async Task GetUsersAsync()
        {
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                _userList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private bool Search(UserResponse user)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private async Task ExportToExcel()
        {
            var base64 = await _userManager.ExportToExcelAsync(_searchString);
            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"{nameof(Users).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
            _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                ? _localizer["Users exported"]
                : _localizer["Filtered Users exported"], Severity.Success);
        }

        private async Task InvokeModal()
        {
            var parameters = new DialogParameters();
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
            var dialog = await _dialogService.ShowAsync<RegisterUserModal>(_localizer["Register New User"], parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await GetUsersAsync();
            }
        }
        private async Task ResetEmailAndPassword(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {

                var parameters = new DialogParameters
                {
                    {
                        nameof(ResetPasswordAndEmail.OldEmail),email
                    }
                };
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };
                var dialog = await _dialogService.ShowAsync<ResetPasswordAndEmail>(_localizer["Reset Password And Email"], parameters, options);
                var result = await dialog.Result;
                if (!result.Canceled)
                {
                    await GetUsersAsync();
                }
            }
        }

        private void ViewProfile(string userId)
        {
            _navigationManager.NavigateTo($"/user-profile/{userId}");
        }

        private void ManageRoles(string userId, string email)
        {
            if (email == "admin@example.com") _snackBar.Add(_localizer["Not Allowed."], Severity.Error);
            else _navigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }
    }
}