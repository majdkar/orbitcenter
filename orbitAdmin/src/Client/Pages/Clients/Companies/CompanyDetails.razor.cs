using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;

namespace SchoolV01.Client.Pages.Clients.Companies
{
    public partial class CompanyDetails
    {
        [Parameter] public int Id { get; set; } = 0;
        [Parameter] public string Type { get; set; } 
        [Inject] private ICompanyManager CompanyManager { get; set; }


        protected GetAllCompaniesResponse CompanyModel = new();

        private bool _loaded;

        protected async override Task OnInitializedAsync()
        {
            await LoadClientInfo();
            //base.OnInitializedAsync();
        }


        private async Task LoadClientInfo()
        {
            var response = await CompanyManager.GetByIdAsync(Id);
            if (response.Succeeded)
            {
                CompanyModel = response.Data;
                _loaded = true;
            }
            else
            {
                _snackBar.Add(_localizer["Error retrieving data"]);
            }
        }

    }
}
