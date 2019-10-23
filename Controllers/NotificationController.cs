using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Services;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IFirebaseService _firebaseService;

        public NotificationController(INotificationService notificationService, IFirebaseService firebaseService)
        {
            _notificationService = notificationService;
            _firebaseService = firebaseService;
        }

        /// <summary>Send notification to specified target group</summary>
        /// <param name="form">Notification form</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendNotificatin([FromBody] DTO_NotificationForm form)
        {
            try
            {
                await _firebaseService.SendNotificationToTopic(form);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}