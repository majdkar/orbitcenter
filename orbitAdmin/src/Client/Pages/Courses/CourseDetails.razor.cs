using SchoolV01.Application.Features.Courses.Queries;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using System.Globalization;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.Courses
{
    public partial class CourseDetails
    {
        [Parameter]
        public int CourseId { get; set; } = 0;
        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Inject]
        public ICourseManager CourseManager { get; set; }
        private GetCourseByIdResponse Model;



        protected async override Task OnInitializedAsync()
        {
            await LoadCourseInfo();
            await base.OnInitializedAsync();
        }
        public async void Back()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }
        private async Task LoadCourseInfo()
        {
            var response = await CourseManager.GetByIdAsync(CourseId);
            if (response.Succeeded)
            {
                Model = response.Data;
            }
            else
            {
                _snackBar.Add(_localizer["Error retrieving data"]);
            }
        }




    }
}