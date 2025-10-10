using SchoolV01.Application.Features.ProductOrders.Queries;
using SchoolV01.Client.Infrastructure.Managers.ProductOrders;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;
using SchoolV01.Application.Features.ProductOrders.Queries.GetAllPaged;
using SchoolV01.Application.Features.ProductOrders.Queries.GetById;
using System.Globalization;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.ProductOrders
{
    public partial class ProductOrderDetails
    {
        [Parameter]
        public int ProductOrderId { get; set; } = 0;
        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Inject]
        public IProductOrderManager ProductOrderManager { get; set; }
        private GetProductOrderByIdResponse Model;



        protected async override Task OnInitializedAsync()
        {
            await LoadProductOrderInfo();
            await base.OnInitializedAsync();
        }
        public async void Back()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }
        private async Task LoadProductOrderInfo()
        {
            var response = await ProductOrderManager.GetByIdAsync(ProductOrderId);
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