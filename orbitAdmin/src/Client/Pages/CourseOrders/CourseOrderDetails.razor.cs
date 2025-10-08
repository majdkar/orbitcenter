using SchoolV01.Application.Features.CourseOrders.Queries;
using SchoolV01.Client.Infrastructure.Managers.CourseOrders;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;
using SchoolV01.Application.Features.CourseOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.CourseOrders.Queries.GetById;
using System.Globalization;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.CourseOrders
{
    public partial class CourseOrderDetails
    {
        [Parameter]
        public int CourseOrderId { get; set; } = 0;
        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Inject]
        public ICourseOrderManager CourseOrderManager { get; set; }
        private GetCourseOrderByIdResponse Model;



        protected async override Task OnInitializedAsync()
        {
            await LoadCourseOrderInfo();
            await base.OnInitializedAsync();
        }
        public async void Back()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }
        private async Task LoadCourseOrderInfo()
        {
            var response = await CourseOrderManager.GetByIdAsync(CourseOrderId);
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