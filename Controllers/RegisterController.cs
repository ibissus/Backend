using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Services;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        /// <summary>Register new user</summary>
        /// <param name="form">Registration form</param>
        /// <returns>Login for the new user</returns>
        /// <response code="200">User created and user's new login returned</response>
        /// <response code="400">User was not created due to form issue</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] DTO_RegisterForm form)
        {
            if (string.IsNullOrEmpty(form.FirstName) || string.IsNullOrEmpty(form.FamilyName)
                || string.IsNullOrEmpty(form.Password))
            {
                return BadRequest("All fields are required");
            }

            if((form.FirstName[0] + form.FamilyName).ToLower() == "superuser")
            {
                return BadRequest("This name is not allowed");
            }

            try
            {
                var login = await _registerService.RegisterNewUser(form);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}