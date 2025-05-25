﻿using SchoolV01.Application.Features.Clients.Companies.Queries.GetById;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetById;
using SchoolV01.Client.Infrastructure.Managers.Clients.Persons;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using SchoolV01.Application.Features.Clients.Persons.Queries.GetAll;

namespace SchoolV01.Client.Pages.Clients.Persons
{
    public partial class PersonDetails
    {
        [Parameter] public int Id { get; set; } = 0;
        [Inject] private IPersonManager PersonManager { get; set; }


        protected GetAllPersonsResponse PersonModel = new();

        private bool _loaded;

        protected async override Task OnInitializedAsync()
        {
            await LoadClientInfo();
            //base.OnInitializedAsync();
        }


        private async Task LoadClientInfo()
        {
            var response = await PersonManager.GetByIdAsync(Id);
            if (response.Succeeded)
            {
                PersonModel = response.Data;
                _loaded = true;
            }
            else
            {
                _snackBar.Add(_localizer["Error retrieving data"]);
            }
        }

    }
}
