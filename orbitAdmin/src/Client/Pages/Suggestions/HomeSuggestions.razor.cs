using SchoolV01.Client.Extensions;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;

using SchoolV01.Client.Infrastructure.Managers.Suggestions;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Requests.Suggestions;
using SchoolV01.Domain.Entities.Suggestions;
using System.Threading;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;

namespace SchoolV01.Client.Pages.Suggestions
{
    public partial class HomeSuggestions
    {
        
        protected async override Task OnInitializedAsync()
        {
        }


     

    }
}
