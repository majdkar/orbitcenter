using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Clients.Persons.Commands.AddEdit;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Application.Requests;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Helpers;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Client.Infrastructure.Managers.Identity.Account;
using SchoolV01.Shared;
using SchoolV01.Shared.Constants;
using SchoolV01.Shared.Constants.Application;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Shared.Constants.Role;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        [Inject] private IClassificationManager ClassificationManager { get; set; }

        [Parameter] public int PersonId { get; set; } = 0;

        private AddEditPersonCommand AddEditPersonModel { get; set; } = new();
      
        private RegisterRequest _registerUserModel = new();
        private FluentValidationValidator _fluentValidationValidator;

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private List<GetAllCountriesResponse> _Countrys = new();
        private List<GetAllClassificationsResponse> _Classification = new();
        private List<GetAllCitiesResponse> _Citys = new();
        private bool _loaded = false;
        IList<IBrowserFile> _files = new List<IBrowserFile>();
        private FileUploadModel fileUploadModel;

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
            await LoadClassificationsAsync();
            
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
                        FullName = person.FullName,
                        Job = person.Job,
                        Qualification = person.Qualification,
                        Mobile1 = person.Mobile1,
                        Mobile2 = person.Mobile2,
                        ClassificationId = person.ClassificationId,
                        CvFileUrl = person.CvFileUrl,
                        PersomImageUrl = person.PersomImageUrl,
                        Sex = person.Sex,
                        BirthDate = person.BirthDate,
                        CountryId = Convert.ToInt32(person.CountryId),
                        CityId = Convert.ToInt32(person.CityId),
                        Phone = person.Phone,
                        Fax = person.Fax,
                        MailBox = person.MailBox,
                        Email = person.Email,
                         FatherName = person.FatherName,
                         FatherNameEn = person.FatherNameEn,
                         FullNameEn = person.FullNameEn,
                          NickName = person.NickName,
                          NickNameEn = person.NickNameEn,
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
        
        private async Task LoadClassificationsAsync()
        {
            var data = await ClassificationManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Classification = data.Data;
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



        private async void SelectFile(InputFileChangeEventArgs e2)
        {
            _files.Clear();
            _files.Add(e2.File);

            if (_files.Count > 0)
            {
                fileUploadModel = await HelperMethods.Save(e2.File);
                this.StateHasChanged();
            }
        }


        private void DeleteFileAsync()
        {
            _files.Clear();
            fileUploadModel = null;
            AddEditPersonModel.CvFileUrl = null;

        }

        string CityToString(int id)
        {
            var student = _Citys.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }  
        
        string ClassificationToString(int? id)
        {
            var student = _Classification.FirstOrDefault(b => b.Id == id);
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
        private async Task<string> UploadFile(IList<IBrowserFile> files, FileUploadModel fileModel, int uploadFileType)
        {
            if (files.Count > 0)
            {
                // Provides a container for content encoded using multipart/form-data MIME type.
                using var content = new MultipartFormDataContent();
                content.Add
                (content: fileModel.Content, name: "\"file\"", fileName: fileModel.Name);
                try
                {
                    var response = await _httpClient.PostAsync($"{EndPoints.FileUpload}/{(int)Enums.FileLocation.MenusFiles}/{uploadFileType}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.getMessage();
                    }
                    else
                    {
                        return String.Empty;
                    }

                }
                catch (Exception e)
                {
                    return String.Empty;
                }

            }
            return String.Empty;
        }


        private async Task SaveAsync()
        {
            if (AddEditPersonModel.Id == 0)
            {
                InitRegisterUserModel();
                var response1 = await _userManager.RegisterUserAsync(_registerUserModel);
                if (response1.Succeeded)
                {

                    var generatedFileName = await UploadFile(_files, fileUploadModel, (int)Enums.UploadFileTypeEnum.File);
                    var fullFilePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedFileName);
                    if (AddEditPersonModel.Id == 0)
                    
                        AddEditPersonModel.CvFileUrl = !String.IsNullOrEmpty(generatedFileName) ? fullFilePath : "";
              
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
                    FirstName = AddEditPersonModel.FullName,
                    LastName = AddEditPersonModel.FullName,
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
                var generatedFileName = await UploadFile(_files, fileUploadModel, (int)Enums.UploadFileTypeEnum.File);
                var fullFilePath = Path.Combine(Constants.UploadFolderName, Enums.FileLocation.MenusFiles.ToString(), generatedFileName);
                if (!String.IsNullOrEmpty(generatedFileName))
                    AddEditPersonModel.CvFileUrl = fullFilePath;
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
         
            
            if (String.IsNullOrEmpty(AddEditPersonModel.FullName))
            {
                _registerUserModel.FirstName = AddEditPersonModel.Phone;
                _registerUserModel.LastName = AddEditPersonModel.Phone;
            }
            else
            {
                _registerUserModel.FirstName = AddEditPersonModel.FullName;
                _registerUserModel.LastName = AddEditPersonModel.FullName;
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


        private void DeletePersonImageAsync()
        {
            AddEditPersonModel.PersomImageUrl = null;
            AddEditPersonModel.PersomImageUploadRequest = new UploadRequest();
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

            return _Countrys.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        private async Task<IEnumerable<int>> SearchCitys(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Citys.Select(x => x.Id);

            return _Citys.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }   
        
        private async Task<IEnumerable<int?>> SearchClassifications(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Classification.Select(x => x?.Id);

            return _Classification.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x?.Id);
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
