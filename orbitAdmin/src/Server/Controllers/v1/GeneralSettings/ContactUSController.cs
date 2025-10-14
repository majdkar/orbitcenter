using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SchoolV01.Application.Features.ContactUs.Commands.AddEdit;

namespace SchoolV01.Server.Controllers.v1.GeneralSettings
{
    public class ContactUSController : BaseApiController<ContactUSController>
    {

        /// <summary>
        /// Send Email For More User
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(SendContactUsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
