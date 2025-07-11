using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

using SchoolV01.Application.Features.Cities.Queries;
using SchoolV01.Application.Features.Classifications.Queries;
using SchoolV01.Application.Features.Clients.Companies.Commands.AddEdit;
using SchoolV01.Application.Features.Countries.Queries;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Requests;
using SchoolV01.Application.Requests.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Client.Helpers;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using SchoolV01.Client.Infrastructure.Managers.Identity.Account;
using SchoolV01.Client.Infrastructure.Managers.Courses;
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



namespace SchoolV01.Client.Pages.Courses
{
    public partial class AddEditCourseSeo
    {

        [Inject] private ICourseSeoManager CourseSeoManager { get; set; }



        [Parameter] public int Id { get; set; } = 0;
        [Parameter] public int CourseId { get; set; } = 0;

        private AddEditCourseSeoCommand AddEditCourseSeoModel { get; set; } = new();
      
        private RegisterRequest _registerUserModel = new();
        private FluentValidationValidator _fluentValidationValidator;

        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

     
        private bool _loaded = false;


        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
        }

        private async Task LoadDataAsync()
        {
            await LoadCourseDetails();
            
        }


     

        public async void Back()
        {
            //_navigationManager.NavigateTo($"/menus/{MenuModel.CategoryId}");
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
        }

        private async Task LoadCourseDetails()
        {
            if (Id != 0)
            {
                var data = await CourseSeoManager.GetByIdAsync(Id);

                if (data.Succeeded)
                {
                    var Course = data.Data;

                    AddEditCourseSeoModel = new AddEditCourseSeoCommand
                    {
                        Id = Course.Id,
                        MetaTitleAr = Course.MetaTitleAr,
                        MetaTitleEn = Course.MetaTitleEn,
                        MetaTitleGe = Course.MetaTitleGe,

                        MetaNameAr = Course.MetaNameAr,
                        MetaNameEn = Course.MetaNameEn,
                        MetaNameGe = Course.MetaNameGe,

                        MetaRobots = Course.MetaRobots,

                        CourseId = Course.CourseId,

                        MetaUrlAr = Course.MetaUrlAr,
                        MetaUrlEn = Course.MetaUrlEn,
                        MetaUrlGe = Course.MetaUrlGe,

                        MetaKeywordsAr = Course.MetaKeywordsAr,
                        MetaKeywordsEn = Course.MetaKeywordsEn,
                        MetaKeywordsGe = Course.MetaKeywordsGe,

                        MetaDescriptionsAr = Course.MetaDescriptionsAr,
                        MetaDescriptionsEn = Course.MetaDescriptionsEn,
                        MetaDescriptionsGe = Course.MetaDescriptionsGe,


                        ImageAlt1Ar = Course.ImageAlt1Ar,
                        ImageAlt1En = Course.ImageAlt1En,
                        ImageAlt1Ge = Course.ImageAlt1Ge,

                        ImageAlt2Ar = Course.ImageAlt2Ar,
                        ImageAlt2En = Course.ImageAlt2En,
                        ImageAlt2Ge = Course.ImageAlt2Ge,

                        ImageAlt3Ar = Course.ImageAlt3Ar,
                        ImageAlt3En = Course.ImageAlt3En,
                        ImageAlt3Ge = Course.ImageAlt3Ge,

                        ImageAlt4Ar = Course.ImageAlt4Ar,
                        ImageAlt4En = Course.ImageAlt4En,
                        ImageAlt4Ge = Course.ImageAlt4Ge,

                    };
                }
            }
            else
                AddEditCourseSeoModel.Id = 0;
        }





        private async Task SaveAsync()
        {
           
                AddEditCourseSeoModel.CourseId = CourseId;
                var response2 = await CourseSeoManager.SaveAsync(AddEditCourseSeoModel);
                if (response2.Succeeded)
                {
                    _snackBar.Add(response2.Messages[0], Severity.Success);
                     Back();
                }
                else
                {
                    foreach (var message in response2.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            

        }
     
    }
}
