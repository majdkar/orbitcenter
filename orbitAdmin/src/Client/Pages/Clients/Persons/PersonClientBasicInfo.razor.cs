using SchoolV01.Application.Features.Clients.Persons.Queries.GetById;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;
using System.Globalization;

namespace SchoolV01.Client.Pages.Clients.Persons
{
    public partial class PersonClientBasicInfo
    {
        [Parameter] public GetAllPersonsResponse Model { get; set; } = new();

        private static bool IsArabic => CultureInfo.CurrentCulture.Name.Contains("ar");
    }
}
