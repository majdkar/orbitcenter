using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetAllPaged;
using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;
using SchoolV01.Client.Infrastructure.Managers.Clients.Companies;
using SchoolV01.Shared.Constants.Permission;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolV01.Client.Pages.Clients.Companies
{
    public partial class CompanyClientBasicInfo 
    {
        [Parameter] public GetAllCompaniesResponse Model { get; set; } = new();

        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");


    }
}