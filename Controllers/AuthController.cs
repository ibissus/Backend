using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AuthController : Controller
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>User Authentication and Authorization</summary>
        /// <returns>JWT authorization token</returns>
        /// <response code="200">Authorized and JWT returned</response>
        /// <response code="400">Authorization failed</response>
        /// <remarks>
        /// System SUPER USER CREDENTIALS
        /// Login: SuperUser
        /// Pass: aaa
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(typeof(DTO_SystemUser), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authorize([FromQuery, Required] string login,[FromQuery, Required] string password, [FromQuery] string firebaseToken = null)
        {
            try
            {
                DTO_SystemUser user = await _userService.SingInAsync(login, password, firebaseToken);

                if (user == null) return BadRequest("Login failed");

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}