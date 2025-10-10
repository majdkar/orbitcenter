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


using SchoolV01.Client.Infrastructure.Managers.ProductOrders;
using SchoolV01.Shared.Constants;
using SchoolV01.Client.Infrastructure.Managers.GeneralSettings;
using System.Globalization;
using SchoolV01.Domain.Entities.Products;
using SchoolV01.Application.Features.Products.Commands.AddEdit;
using SchoolV01.Client.Infrastructure.Managers.Products;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetAll;
using SchoolV01.Domain.Entities.GeneralSettings;
using System.Threading;
using SchoolV01.Shared.Constants.Clients;
using SchoolV01.Domain.Entities.Orders;

namespace SchoolV01.Client.Pages.ProductOrders
{
    public partial class AddEditProductOrder
    {
        [Inject] private IProductOrderManager ProductOrderManager { get; set; }
        [Inject] private IProductManager ProductManager { get; set; }
        [Inject] private IPersonManager PersonManager { get; set; }
        [Inject] private ICompanyManager CompanyManager { get; set; }

        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Parameter] public int ProductOrderId { get; set; } = 0;

        private AddEditProductOrderCommand AddEditProductOrderModel { get; set; } = new();

        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private FluentValidationValidator _fluentValidationValidator;

        private List<GetAllPersonsResponse> _persons = new();

        private List<GetAllCompaniesResponse> _companies = new();

        private List<GetAllProductsResponse> _Products = new();



        private bool _isProcessing = false;
   
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private bool _loaded = false;

        private int SelectedProductId;
        private int SelectedQuantity = 1;
        private decimal SelectedPrice = 0;

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

             LoadProductOrderDetails(),
             LoadPerson(),
             LoadCompanies(),
             LoadProduct()
            );
        }


        private async Task AddItem()
        {
            if (SelectedProductId == 0 || SelectedQuantity <= 0) return;

            var product = await ProductManager.GetByIdAsync(SelectedProductId);
            if (product == null) return;


            var productEntity = new Product
            {
                Id = product.Data.Id,
                NameAr = product.Data.NameAr,
                NameEn = product.Data.NameEn,
                // أضف أي خصائص أخرى ضرورية
            };

            var item = new ProductOrderItem
            {
                ProductId = product.Data.Id,
                Product = productEntity,
                Quantity = SelectedQuantity,
                UnitPrice = SelectedPrice > 0 ? SelectedPrice : 0
            };

            AddEditProductOrderModel.Items.Add(item);

            // إعادة حساب المجموع الكلي
            AddEditProductOrderModel.TotalPrice = AddEditProductOrderModel.Items.Sum(i => i.UnitPrice * i.Quantity);

            // إعادة تعيين الحقول
            SelectedProductId = 0;
            SelectedQuantity = 1;
            SelectedPrice = 0;
        }

        private void RemoveItem(ProductOrderItem item)
        {
            AddEditProductOrderModel.Items.Remove(item);
            AddEditProductOrderModel.TotalPrice = AddEditProductOrderModel.Items.Sum(i => i.UnitPrice * i.Quantity);
        }


        private async Task LoadProduct()
        {

            var data = await ProductManager.GetAllAsync();
            if (data.Succeeded)
            {
                _Products = data.Data.ToList();
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
        
        private async Task<IEnumerable<int>> SearchProducts(string value, CancellationToken token)
        {
            await Task.Delay(5);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return _Products.Select(x => x.Id);

            return _Products.Where(x => x.NameAr.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.NameEn.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                .Select(x => x.Id);
        }

        string ProductToString(int id)
        {
            var student = _Products.FirstOrDefault(b => b.Id == id);
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

        private async Task LoadProductOrderDetails()
        {
            if (ProductOrderId != 0)
            {
                var data = await ProductOrderManager.GetByIdAsync(ProductOrderId);
                if (data.Succeeded)
                {
                    var ProductOrder = data.Data;
                    AddEditProductOrderModel = new AddEditProductOrderCommand
                    {
                        Id = ProductOrder.Id,
                         ClientId = ProductOrder.ClientId,
                          Notes = ProductOrder.Notes,
                          OrderDate = ProductOrder.OrderDate,
                          OrderNumber = ProductOrder.OrderNumber,
                          PaymentStatus = ProductOrder.PaymentStatus,
                           Items = ProductOrder.Items,
                            TotalPrice = ProductOrder.TotalPrice,
                          Status = ProductOrder.Status,
                           ClientType = ProductOrder.ClientType,
                    };
                }
                    
            }
            else
            {
                AddEditProductOrderModel.Id = 0;
            }

        }

      
       
    
        private async Task SaveAsync()
        {
            _isProcessing = true;


            AddEditProductOrderModel.Status = OrderStatusEnum.Accepted.ToString();
            var response = await ProductOrderManager.SaveAsync(AddEditProductOrderModel);
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
