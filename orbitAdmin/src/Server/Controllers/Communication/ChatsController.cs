 using SchoolV01.Application.Interfaces.Services;
using SchoolV01.Domain.Models.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SchoolV01.Domain.Interfaces.Chat;
using SchoolV01.Shared.Constants.Permission;
using Microsoft.AspNetCore.SignalR.Client;
using SchoolV01.Shared.Constants.Application;
using Microsoft.AspNetCore.SignalR;
using SchoolV01.Server.Hubs;
using System.Collections.Concurrent;
using System.Security.Claims;
using Azure.Core;
using Microsoft.AspNetCore.Components;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Application.Interfaces.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SchoolV01.Server.Controllers.Communication
{
    [AllowAnonymous]
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IUnitOfWork<int> uow;

        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatService _chatService;
        private HubConnection HubConnection { get; set; }
        public ChatsController(ICurrentUserService currentUserService, IChatService chatService, IHubContext<SignalRHub> hubContext, IUnitOfWork<int> uow)
        {
            _currentUserService = currentUserService;
            _chatService = chatService;
            _hubContext = hubContext;
            this.uow = uow;

        }

        /// <summary>
        /// Get user wise chat history
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns>Status 200 OK</returns>
        //Get user wise chat history
        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetChatHistoryAsync(string contactId)
        {
            return Ok(await _chatService.GetChatHistoryAsync(_currentUserService.UserId, contactId));
        }
        /// <summary>
        /// get available users
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //get available users - sorted by date of last message if exists
        [HttpGet("users")]
        public async Task<IActionResult> GetChatUsersAsync()
        {
            return Ok(await _chatService.GetChatUsersAsync(_currentUserService.UserId));
        }

        /// <summary>
        /// Save Chat Message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("Message")]
        public async Task<IActionResult> SaveMessageAsync(ChatHistory<IChatUser> message)
        {
            message.FromUserId = message.FromUserId;
            message.ToUserId = message.ToUserId;
            message.CreatedDate = DateTime.Now;
            string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";


            HubConnection = new HubConnectionBuilder()
                                .WithUrl(hostDomain + "/signalRHub")
                                .Build();

            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
            await HubConnection.SendAsync(ApplicationConstants.SignalR.SendMessage, message, "userName");

            return Ok(await _chatService.SaveMessageAsync(message));
        }



        /// <summary>
        /// Save Chat Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userId"></param>

        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("SaveTest")]
        public async Task<IActionResult> SaveTestAsync([FromBody] string message,string userId)
        {

            //string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";


            //HubConnection = new HubConnectionBuilder()
            //                    .WithUrl(hostDomain + "/signalRHub")
            //                    .Build();
            //if (HubConnection.State == HubConnectionState.Disconnected)
            //{
            //    await HubConnection.StartAsync();
            //}
            //await HubConnection.SendAsync(ApplicationConstants.SignalR.SendMessage, message,userId);


            ////await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            //return Ok(new { status = "Message sent" });


            var connectionId = SignalRHub.GetConnectionId(userId);
            if (connectionId != null)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                //await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
               
                
                return Ok("Message sent");
            }

            return NotFound("User not connected");

        }



        /// <summary>
        /// Start Hub
        /// </summary>
        /// <param name="userId"></param>

        /// <returns>Status 200 OK</returns>
        //save chat message
        [HttpPost("StartHub")]
        public async Task<IActionResult> StartHub(string userId)
        {

            if (userId != null)
            {
                string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";

                HubConnection = new HubConnectionBuilder()
                                    .WithUrl(hostDomain + "/signalRHub")
                                    .Build();
                if (HubConnection.State == HubConnectionState.Disconnected)
                {
                    await HubConnection.StartAsync();
                }

                await HubConnection.SendAsync(ApplicationConstants.SignalR.OnConnect, userId);

                return Ok("Hub Start");
            }

            return NotFound("Hub No Start");

        }





        //=============================================================

        //private static ConcurrentDictionary<string, bool> TrackingUsers = new();

        //[HttpPost("start")]
        //public async Task<IActionResult> StartTrack(string userId)
        //{
        //    // string hostDomain = $"{Request.Scheme}://{Request.Host.Value}";

        //    // HubConnection = new HubConnectionBuilder()
        //    //                     .WithUrl(hostDomain + "/signalRHub", options =>
        //    //                     {
        //    //                         options.AccessTokenProvider = () => Task.FromResult(accessToken);
        //    //                     })
        //    //           .WithAutomaticReconnect().Build();
        //    // if (HubConnection.State == HubConnectionState.Disconnected)
        //    // {
        //    //     await HubConnection.StartAsync();
        //    // }
        //    //await HubConnection.SendAsync("StartTrack");

        //    var connectionId = SignalRHub.GetConnectionId(userId);
        //    if (connectionId != null)
        //    {
        //        await _hubContext.Clients.Client(connectionId).SendAsync("StartTrack");
        //        //await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);


        //        return Ok(new { message = "Tracking started" });
        //    }

        //    return Ok(new { message = "No" });
        //}

        //[HttpPost("stop")]
        //public async Task<IActionResult> StopTrack(string userId)
        //{
        //    //var connectionId = SignalRHub.GetConnectionId(userId);
        //    //if (connectionId != null)
        //    //{
        //        //await _hubContext.Clients.Client(connectionId).SendAsync("TrackStopped");
        //        await _hubContext.Clients.All.SendAsync("TrackStopped");


        //        return Ok(new { message = "Tracking stopped" });
        //    //}

        //    //return Ok(new { message = "No" });
        //}

        //[HttpGet("status")]
        //public IActionResult GetStatus()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    bool isTracking = TrackingUsers.ContainsKey(userId);
        //    return Ok(new { isTracking });
        //}





        //=======================================

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VehicleLocation location)
        {
            var vehicleId = User.FindFirst("userId")?.Value;
            if (vehicleId == null)
            {
                return Forbid();
            }

            uow.Add(location);
            await uow.CommitAsync();

            await _hubContext.Clients.Group(location.VehicleId).SendAsync("ReceiveLocation", location.VehicleId, location.Latitude, location.Longitude);
            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var vehicleId = User.FindFirst("UserId")?.Value;
            if (vehicleId == null)
            {
                return Forbid();
            }

            var history = await uow.Query<VehicleLocation>()
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.Timestamp)
                .Take(100)
                .ToListAsync();

            return Ok(history);
        }

    }
}