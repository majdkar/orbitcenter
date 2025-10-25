using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


using SchoolV01.Application.Requests;
using SchoolV01.Client.Extensions;


using SchoolV01.Client.Infrastructure.Managers.CourseOrders;
using SchoolV01.Shared.Constants;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using System.Globalization;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Client.Infrastructure.Managers.Courses;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Threading;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings.PayType;
using SchoolV01.Application.Features.PayTypes.Queries.GetAll;

namespace SchoolV01.Client.Pages.CourseOrders
{
    public partial class AddEditCourseOrder
    {
        [Inject] private ICourseOrderManager CourseOrderManager { get; set; }
        [Inject] private ICourseManager CourseManager { get; set; }
        [Inject] private IPersonManager PersonManager { get; set; }
        [Inject] private ICompanyManager CompanyManager { get; set; }
        [Inject] private IPayTypeManager PayTypeManager { get; set; }

        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Parameter] public int CourseOrderId { get; set; } = 0;

        private AddEditCourseOrderCommand AddEditCourseOrderModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;

        private List<GetAllPersonsResponse> _persons = new();

        private List<GetAllCompaniesResponse> _companies = new();

        private List<GetAllCoursesResponse> _courses = new();

        private List<GetAllPayTypesResponse> _payTypes = new();



        private bool _isProcessing = false;
   
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool _loaded = false;


        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
     

        }
        public async void Cancel()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);
            //MudDialog.Cancel();

        }
        private async Task LoadDataAsync()
        {
            await Task.WhenAll(

             LoadCourseOrderDetails(),
             LoadPerson(),
             LoadCompanies(),
             LoadCourse(),
             LoadPayType()
            );
        }

        private async Task LoadCourse()
        {

            var data = await CourseManager.GetAllAsync();
            if (data.Succeeded)
            {
                _courses = data.Data.ToList();
            }
        }

        private async Task LoadPayType()
        {

            var data = await PayTypeManager.GetAllAsync();
            if (data.Succeeded)
            {
                _payTypes = data.Data.ToList();
            }
        }

        private async Task LoadCompanies()
        {
            var data = await CompanyManager.GetAllAcceptedAsync();
            if (data.Succeeded)
            {
                _companies = data.Data.ToList();
            }
        }

        private async Task LoadPerson()
        {
            var data = await PersonManager.GetAllAsync();
            if (data.Succeeded)
            {
                _persons = data.Data.ToList();
            }
        }



        private async Task<IEnumerable<int>> SearchCompanies(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _companies.Select(x => x.ClientId);

            return _companies.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.ClientId);
        }

        private async Task<IEnumerable<int>> SearchPersons(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _persons.Select(x => x.ClientId);

            return _persons.Where(x => x.FullName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.FullNameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.ClientId);
        }
        
        private async Task<IEnumerable<int>> SearchCourses(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _courses.Select(x => x.Id);

            return _courses.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }
        
        private async Task<IEnumerable<int?>> SearchPayType(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _payTypes.Select(x => x?.Id);

            return _payTypes.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x?.Id);
        }

        string CourseToString(int id)
        {
            var student = _courses.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }

        string PayTypeToString(int? id)
        {
            var student = _payTypes.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameEn} - {student.NameAr}";
        }
        
        string PersonToString(int id)
        {
            var student = _persons.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.FullName} - {student.FullNameEn}";
        }  
        string CompanyToString(int id)
        {
            var student = _companies.FirstOrDefault(b => b.Id == id);
            if (student is null)
                return string.Empty;

            return $"{student.NameAr} - {student.NameEn}";
        }

        private async Task LoadCourseOrderDetails()
        {
            if (CourseOrderId != 0)
            {
                var data = await CourseOrderManager.GetByIdAsync(CourseOrderId);
                if (data.Succeeded)
                {
                    var CourseOrder = data.Data;
                    AddEditCourseOrderModel = new AddEditCourseOrderCommand
                    {
                        Id = CourseOrder.Id,
                         ClientId = CourseOrder.ClientId,
                          CourseId = CourseOrder.CourseId,
                          Notes = CourseOrder.Notes,
                          OrderDate = CourseOrder.OrderDate,
                          OrderNumber = CourseOrder.OrderNumber,
                          PaymentStatus = CourseOrder.PaymentStatus,
                          Price = CourseOrder.Price,
                          Status = CourseOrder.Status,
                           PayTypeId =CourseOrder.PayTypeId,
                           PaymentTransactionNumber = CourseOrder.PaymentTransactionNumber,
                    };
                }
                    
            }
            else
            {
                AddEditCourseOrderModel.Id = 0;
            }

        }

      
       
    
        private async Task SaveAsync()
        {
            _isProcessing = true;


            AddEditCourseOrderModel.Status = OrderStatusEnum.Accepted.ToString();
            var response = await CourseOrderManager.SaveAsync(AddEditCourseOrderModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                Cancel();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
            _isProcessing = false;
        }

    }
}
