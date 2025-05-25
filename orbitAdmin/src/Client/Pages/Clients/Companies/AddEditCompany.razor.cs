using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;

using SchoolV01.Application.Requests;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;

using SchoolV01.Client.Infrastructure.Managers.Identity.Account;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using SchoolV01.Application.Features.Clients.Companies.Queries;

using System.Threading;
using SchoolV01.Shared.Constants.Role;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Application.Features.Cities.Queries;


namespace SchoolV01.Client.Pages.Clients.Companies
{
    public partial class AddEditCompany
    {
        [Inject] private IAccountManager AccountManager { get; set; }

        [Inject] private ICompanyManager CompanyManager { get; set; }

        [Inject] private ICityManager CityManager { get; set; }

  
        [Inject] private ICountryManager CountryManager { get; set; }

        [Parameter] public int CompanyId { get; set; } = 0;

        private AddEditCompanyCommand AddEditCompanyModel { get; set; } = new();

        private RegisterRequest _registerUserModel = new();
        private FluentValidationValidator _fluentValidationValidator;

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private List<GetAllCountriesResponse> _Countrys = new();
        private List<GetAllCitiesResponse> _Citys = new();
        //TextEditorConfig editorAr = new TextEditorConfig("#editorAr");

        //TextEditorConfig editorEn = new TextEditorConfig("#editorEn");

       
        private bool _processing = false;
        private bool _loaded = false;

  

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
          
            await Task.WhenAll(
                LoadCompanyDetails(),
                LoadCountrysAsync(), LoadCitysAsync());
            
            
        }

        

        private async Task LoadCompanyDetails()
        {
            if (CompanyId != 0)
            {
                var data = await CompanyManager.GetByIdAsync(CompanyId);
                if (data.Succeeded)
                {
                    var company = data.Data;
                    AddEditCompanyModel = new AddEditCompanyCommand
                    {
                        Id = company.Id,
                        NameAr = company.NameAr,
                        NameEn = company.NameEn,
                        CountryId = Convert.ToInt32(company.CountryId),
                        CityId = Convert.ToInt32(company.CityId),
                        LicenseIssuingDate = company.LicenseIssuingDate,
                        ResponsiblePersonNameAr = company.ResponsiblePersonNameAr,
                        ResponsiblePersonNameEn = company.ResponsiblePersonNameEn,
                        ResponsiblePersonMobile = company.ResponsiblePersonMobile,
                        Phone = company.Phone,
                        Email = company.Email,
                        Address = company.Address,
                        Website = company.Website,
                        AdditionalInfo = company.AdditionalInfo,
                        CompanyImageUrl = company.CompanyImageUrl,
                        UserId = company.UserId,
                        Status = company.Status,
                    };
                    

                
                }
            }
            else
            {
                AddEditCompanyModel.Id = 0;
            }

        }

        private async void OnCountryChanged(int value)
        {

            if (Convert.ToInt32(value) != 0)
            {
               _Citys = _Citys.Where(x => x.CountryId == value).ToList();
                //StateHasChanged();
            }


        }



        private async Task LoadCountrysAsync()
        {
            
            var data = await CountryManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Countrys = data.Data;
            }
        }

        private async Task LoadCitysAsync()
        {

            var data = await CityManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Citys = data.Data;
            }


        }




        string CityToString(int id)
        {
            var student = _Citys.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }

        

        string CountryToString(int id)
        {
            var student = _Countrys.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }



        private async Task SaveAsync()
        {
            if (String.IsNullOrEmpty(_registerUserModel.Password))
            {
                _snackBar.Add("Password is required", Severity.Error);
                return;
            }
            _processing = true;

            if (AddEditCompanyModel.Id == 0)
            {
                InitRegisterUserModel();
                var response1 = await _userManager.RegisterUserAsync(_registerUserModel);
                if (response1.Succeeded)
                {
                    AddEditCompanyModel.UserId = response1.Messages[0];
                    AddEditCompanyModel.Status = ClientStatusEnum.Accepted.ToString();
                    var response2 = await CompanyManager.SaveAsync(AddEditCompanyModel);
                    if (response2.Succeeded)
                    {
                        _snackBar.Add(response2.Messages[0], Severity.Success);
                        RedirectToCompaniesPage();
                    }
                    else
                    {
                        foreach (var message in response2.Messages)
                        {
                            _snackBar.Add(message, Severity.Error);
                        }
                    }
                }
                else
                {
                    foreach (var message in response1.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
            else
            {
                
                //update user account
                var request1 = new UpdateProfileByAdminRequest
                {
                    UserId = AddEditCompanyModel.UserId,
                    FirstName = AddEditCompanyModel.NameEn,
                    LastName = AddEditCompanyModel.NameEn,
                    Email = AddEditCompanyModel.Email,
                    PhoneNumber = AddEditCompanyModel.Phone,
                };
                await AccountManager.UpdateProfileByAdminAsync(request1);

                //change password
                if (!string.IsNullOrEmpty(_registerUserModel.Password) && !string.IsNullOrEmpty(_registerUserModel.ConfirmPassword) && _registerUserModel.Password == _registerUserModel.ConfirmPassword)
                {
                    var request2 = new ChangePasswordByAdminRequest
                    {
                        UserId = AddEditCompanyModel.UserId,
                        NewPassword = _registerUserModel.Password,
                        ConfirmNewPassword = _registerUserModel.ConfirmPassword,
                    };
                    await AccountManager.ChangePasswordByAdminAsync(request2);
                }

                //update company info
                var response3 = await CompanyManager.SaveAsync(AddEditCompanyModel);
                if (response3.Succeeded)
                {
                    _snackBar.Add(response3.Messages[0], Severity.Success);
                    RedirectToCompaniesPage();
                }
                else
                {
                    foreach (var message in response3.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
            _processing = false;
        }

        private void RedirectToCompaniesPage()
        {
            _navigationManager.NavigateTo($"/companies");
        }

        private void InitRegisterUserModel()
        {
            _registerUserModel.IsActive = true;
            _registerUserModel.AutoConfirmEmail = true;
            _registerUserModel.ClientType = RoleConstants.BasicRole;

            if (String.IsNullOrEmpty(AddEditCompanyModel.NameEn))
            {
                _registerUserModel.FirstName = AddEditCompanyModel.Phone;
                _registerUserModel.LastName = AddEditCompanyModel.Phone;
            }
            else
            {
                _registerUserModel.FirstName = AddEditCompanyModel.NameEn;
                _registerUserModel.LastName = AddEditCompanyModel.NameEn;
            }

            _registerUserModel.Email = AddEditCompanyModel.Email;
            _registerUserModel.UserName = AddEditCompanyModel.Phone;
            _registerUserModel.PhoneNumber = AddEditCompanyModel.Phone;
        }

        private IBrowserFile _imageFile;
        private IBrowserFile _licenseFile;
        private IBrowserFile _cvVFile;
        private IBrowserFile _bannerFile;

        private async Task UploadCompanyImage(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyModel.CompanyImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditCompanyModel.CompanyImageUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Student, Extension = extension };
            }
        }

        //private async Task UploadLicenseImage(InputFileChangeEventArgs e)
        //{
        //    _licenseFile = e.File;
        //    if (_licenseFile != null)
        //    {
        //        var extension = Path.GetExtension(_licenseFile.Name);
        //        var format = "image/png";
        //        var imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
        //        var buffer = new byte[imageFile.Size];
        //        await imageFile.OpenReadStream().ReadAsync(buffer);
        //        AddEditCompanyModel.LicenseImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
        //        AddEditCompanyModel.LicenseImageUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Company, Extension = extension };
        //    }
        //}

        private async Task UploadCompanyFile(InputFileChangeEventArgs e)
        {
            _cvVFile = e.File;
            if (_cvVFile != null)
            {
                var extension = Path.GetExtension(_cvVFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditCompanyModel.CompanyFileUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                AddEditCompanyModel.CompanyFileUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Student, Extension = extension };
            }
        }

  

        private void DeleteCompanyImageAsync()
        {
            AddEditCompanyModel.CompanyImageUrl = null;
            AddEditCompanyModel.CompanyImageUploadRequest = new UploadRequest();
        }

        private void DeleteCompanyFileAsync()
        {
            AddEditCompanyModel.CompanyFileUrl = null;
            AddEditCompanyModel.CompanyFileUploadRequest = new UploadRequest();
        }


        private async Task<IEnumerable<int>> SearchCountrys(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Countrys.Select(x => x.Id);

            return _Countrys.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        private async Task<IEnumerable<int>> SearchCitys(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Citys.Select(x => x.Id);

            return _Citys.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }


        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

    }
}
