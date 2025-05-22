using SchoolV01.Application.Features.Products.Queries;
using SchoolV01.Client.Infrastructure.Managers.Products;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

using System.Linq;
using SchoolV01.Application.Features.Products.Queries.GetAllPaged;
using SchoolV01.Application.Features.Products.Queries.GetById;
using System.Globalization;
using Microsoft.JSInterop;

namespace SchoolV01.Client.Pages.Products
{
    public partial class ProductDetails
    {
        [Parameter]
        public int ProductId { get; set; } = 0;
        private bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar-");

        [Inject]
        public IProductManager ProductManager { get; set; }
        private GetProductByIdResponse Model;



        protected async override Task OnInitializedAsync()
        {
            await LoadProductInfo();
            await base.OnInitializedAsync();
        }
        public async void Back()
        {
            await _jsRuntime.InvokeVoidAsync("history.back", -1);


        }
        private async Task LoadProductInfo()
        {
            var response = await ProductManager.GetByIdAsync(ProductId);
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