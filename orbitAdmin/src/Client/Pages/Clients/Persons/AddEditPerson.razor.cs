using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Application.Requests;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Client.Infrastructure.Managers.Identity.Account;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Constants.Role;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;



namespace SchoolV01.Client.Pages.Clients.Persons
{
    public partial class AddEditPerson
    {
        [Inject] private IAccountManager AccountManager { get; set; }

        [Inject] private IPersonManager PersonManager { get; set; }

        [Inject] private ICityManager CityManager { get; set; }

        [Inject] private ICountryManager CountryManager { get; set; }

        [Parameter] public int PersonId { get; set; } = 0;

        private AddEditPersonCommand AddEditPersonModel { get; set; } = new();
      
        private RegisterRequest _registerUserModel = new();
        private FluentValidationValidator _fluentValidationValidator;

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private List<GetAllCountriesResponse> _Countrys = new();
        private List<GetAllCitiesResponse> _Citys = new();
        private List<GetAllCitiesResponse> _sections = new();
        private List<GetAllCitiesResponse> _CitysToView = new();
        private bool _loaded = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadCitysAsync();
            await LoadPersonDetails();
            await LoadCountrysAsync();
            
        }


        //Google Map Functions
     


        private async Task LoadPersonDetails()
        {
            if (PersonId != 0)
            {
                var data = await PersonManager.GetByIdAsync(PersonId);

                if (data.Succeeded)
                {
                    var person = data.Data;

                    AddEditPersonModel = new AddEditPersonCommand
                    {
                        Id = person.Id,
                        FullNameAr = person.FullNameAr,
                        FullNameEn = person.FullNameEn,
                        Sex = person.Sex,
                        BirthDate = person.BirthDate,
                        CountryId = Convert.ToInt32(person.CountryId),
                        CityId = Convert.ToInt32(person.CityId),
                        Phone = person.Phone,
                        Fax = person.Fax,
                        MailBox = person.MailBox,
                        Email = person.Email,
                        
                        Address = person.Address,
                        AdditionalInfo = person.AdditionalInfo,
                        Status = person.Client.Status ?? ClientStatusEnum.Pending.ToString(),
                        UserId = person.UserId,
                    };
                }
            }
            else
                AddEditPersonModel.Id = 0;
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
            if (AddEditPersonModel.Id == 0)
            {
                InitRegisterUserModel();
                var response1 = await _userManager.RegisterUserAsync(_registerUserModel);
                if (response1.Succeeded)
                {
                    AddEditPersonModel.UserId = response1.Messages[0];
                    AddEditPersonModel.Status = ClientStatusEnum.Accepted.ToString();
                    var response2 = await PersonManager.SaveAsync(AddEditPersonModel);
                    if (response2.Succeeded)
                    {
                        _snackBar.Add(response2.Messages[0], Severity.Success);
                        RedirectToPersonsPage();
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
                //update email in Users 
                var request1 = new UpdateProfileByAdminRequest
                {
                    UserId = AddEditPersonModel.UserId,
                    FirstName = AddEditPersonModel.FullNameEn,
                    LastName = AddEditPersonModel.FullNameEn,
                    Email = AddEditPersonModel.Email,
                    PhoneNumber = AddEditPersonModel.Phone,
                };
                await AccountManager.UpdateProfileByAdminAsync(request1);

                //change password
                if (!string.IsNullOrEmpty(_registerUserModel.Password) && !string.IsNullOrEmpty(_registerUserModel.ConfirmPassword) && _registerUserModel.Password == _registerUserModel.ConfirmPassword)
                {
                    var request2 = new ChangePasswordByAdminRequest
                    {
                        UserId = AddEditPersonModel.UserId,
                        NewPassword = _registerUserModel.Password,
                        ConfirmNewPassword = _registerUserModel.ConfirmPassword,
                    };
                 await AccountManager.ChangePasswordByAdminAsync(request2);
                }

                //update person info
                var response = await PersonManager.SaveAsync(AddEditPersonModel);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    RedirectToPersonsPage();
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

        private void RedirectToPersonsPage()
        {
            _navigationManager.NavigateTo($"/individuals");
        }

        private void InitRegisterUserModel()
        {
            _registerUserModel.IsActive = true;
            _registerUserModel.AutoConfirmEmail = true;

            _registerUserModel.ClientType = RoleConstants.BasicRole;
         
            
            if (String.IsNullOrEmpty(AddEditPersonModel.FullNameEn))
            {
                _registerUserModel.FirstName = AddEditPersonModel.Phone;
                _registerUserModel.LastName = AddEditPersonModel.Phone;
            }
            else
            {
                _registerUserModel.FirstName = AddEditPersonModel.FullNameEn;
                _registerUserModel.LastName = AddEditPersonModel.FullNameEn;
            }

            _registerUserModel.Email = AddEditPersonModel.Email;
            _registerUserModel.UserName = AddEditPersonModel.Phone;
            _registerUserModel.PhoneNumber = AddEditPersonModel.Phone;
        }

        private IBrowserFile _imageFile;
        private IBrowserFile _identifierFile;
        private IBrowserFile _cvVFile;

        private async Task UploadPersonImage(InputFileChangeEventArgs e)
        {
            _imageFile = e.File;
            if (_imageFile != null)
            {
                var extension = Path.GetExtension(_imageFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditPersonModel.PersomImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditPersonModel.PersomImageUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Student, Extension = extension };
            }
        }

        private async Task UploadIdImage(InputFileChangeEventArgs e)
        {
            _identifierFile = e.File;
            if (_identifierFile != null)
            {
                var extension = Path.GetExtension(_identifierFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditPersonModel.IdentifierImageUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                AddEditPersonModel.IdentifierImageUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Student, Extension = extension };
            }
        }

        private async Task UploadPersonFile(InputFileChangeEventArgs e)
        {
            _cvVFile = e.File;
            if (_cvVFile != null)
            {
                var extension = Path.GetExtension(_cvVFile.Name);
                var format = "image/png";
                var imageFile = await e.File.RequestImageFileAsync(format, 600, 600);
                var buffer = new byte[imageFile.Size];
                await imageFile.OpenReadStream().ReadAsync(buffer);
                AddEditPersonModel.CvFileUrl = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                AddEditPersonModel.CvFileUploadRequest = new UploadRequest { Data = buffer, UploadType = Application.Enums.UploadType.Student, Extension = extension };
            }
        }

        private void DeletePersonImageAsync()
        {
            AddEditPersonModel.PersomImageUrl = null;
            AddEditPersonModel.PersomImageUploadRequest = new UploadRequest();
        }

        private void DeleteIdImageAsync()
        {
            AddEditPersonModel.IdentifierImageUrl = null;
            AddEditPersonModel.IdentifierImageUploadRequest = new UploadRequest();
        }

        private void DeletePersonFileAsync()
        {
            AddEditPersonModel.CvFileUrl = null;
            AddEditPersonModel.CvFileUploadRequest = new UploadRequest();
        }

        private async void OnCountryChanged(int value)
        {

            AddEditPersonModel.CountryId = value;
            //_CitysToView.Clear();
            if (Convert.ToInt32(AddEditPersonModel.CountryId) != 0)
            {
                _Citys =  _Citys.Where(x => x.CountryId == AddEditPersonModel.CountryId).ToList();
                StateHasChanged();
            }


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

        private async Task<IEnumerable<int>> SearchSections(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _sections.Select(x => x.Id);

            return _sections.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase))
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
