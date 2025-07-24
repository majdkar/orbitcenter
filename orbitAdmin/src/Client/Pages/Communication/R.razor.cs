using SchoolV01.Domain.Models.Chat;
using SchoolV01.Application.Responses.Identity;
using SchoolV01.Client.Extensions;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolV01.Domain.Interfaces.Chat;
using SchoolV01.Client.Infrastructure.Managers.Communication;
using SchoolV01.Shared.Constants.Storage;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchoolV01.Client.Pages.Communication
{
    public partial class R
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<string> messages = new(); // قائمة لتخزين الرسائل

        private HubConnection? _hubConnection;

        private bool _isTracking = false;

        protected override async Task OnInitializedAsync()
        {

            //HubConnection = HubConnection.TryInitialize(_navigationManager);
            //if (HubConnection.State == HubConnectionState.Disconnected)
            //{
            //    await HubConnection.StartAsync();
            //}
            var accessToken = await _localStorage.GetItemAsync<string>("authToken");

            _hubConnection = new HubConnectionBuilder()
                      .WithUrl(_navigationManager.ToAbsoluteUri("/SignalRHub"), options =>
                      {
                          options.AccessTokenProvider = () => Task.FromResult(accessToken);
                      })
                      .WithAutomaticReconnect()
                      .Build();

            await _hubConnection.StartAsync();

            _hubConnection.On<string>("ReceiveMessage", (message) =>
            {

                messages.Add(message); // حفظ الرسالة
                InvokeAsync(StateHasChanged); // إعادة تحديث الواجهة                // التعامل مع الرسالة
                Console.WriteLine($"Received: {message}");
            });





            _hubConnection.On<string>("TrackStarted", message =>
            {
                _isTracking = true;
                StateHasChanged();
            });

            _hubConnection.On<string>("TrackStopped", message =>
            {
                _isTracking = false;
                StateHasChanged();
            });

            _hubConnection.On<string, double, double>("ReceiveLocation", (userId, lat, lng) =>
            {
                // Handle location updates here
            });

        }

        private async Task StartTracking()
        {
            await _hubConnection.InvokeAsync("StartTrack");
        }

        private async Task StopTracking()
        {
            await _hubConnection.InvokeAsync("StopTrack");
        }




    }
}

//_hubConnection = new HubConnectionBuilder()
//          .WithUrl(Navigation.ToAbsoluteUri("/vehiclehub"))
//          .WithAutomaticReconnect()
//          .Build();

//_hubConnection.On<int, double, double>("ReceiveLocation", (vehicleId, lat, lng) =>
//{
//    message = $"Vehicle {vehicleId} at: {lat}, {lng}";
//    StateHasChanged();
//});

//await _hubConnection.StartAsync();