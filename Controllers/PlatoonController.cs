using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.Filters;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class PlatoonController : Controller
    {
        private readonly IPlatoonService _platoonService;

        public PlatoonController(IPlatoonService platoonService)
        {
            _platoonService = platoonService;
        }

        /// <summary>Get platoon groups of the company</summary>
        /// <param name="companyId">Company number</param>
        /// <returns>List of platoon groups</returns>
        /// <response code="200">Platoon list</response>
        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(List<Pluton>), StatusCodes.Status200OK)]
        [GroupMember(GroupType.Company)]
        public async Task<IActionResult> GetPlatoonList([FromRoute] int companyId)
        {
            return Ok(await _platoonService.GetPlatoonList(companyId));
        }

        /// <summary>Get platoon details</summary>
        /// <param name="companyId">Company number</param>
        /// <param name="platoonId">Platoon number</param>
        /// <returns>Platoon group details</returns>
        /// <response code="200">Platoon details</response>
        /// <response code="400">Error processing request</response>
        /// <response code="404">Platoon does not exist</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Pluton>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [GroupMember(GroupType.Platoon)]
        public async Task<IActionResult> GetPlatoonDetails([FromQuery, Required] int companyId, [FromQuery, Required] int platoonId)
        {
            Pluton platoon;

            try
            {
                platoon = await _platoonService.GetPlatoonDetails(companyId, platoonId, User.IsInRole(PlatoonCommanderRoleName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            if(platoon == null)
            {
                return NotFound();
            }

            return Ok(platoon);
        }

        /// <summary>Create new platoon group</summary>
        /// <param name="companyId">Company number</param>
        /// <param name="platoonId">Platoon nuber</param>
        /// <response code="200">Platoon created</response>
        /// <response code="400">Error processing request</response>
        [HttpPost("{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [GroupMember(GroupType.Company)]
        public async Task<IActionResult> CreatePlatoon([FromRoute] int companyId, [FromQuery, Required] int platoonId)
        {
            try
            {
                await _platoonService.CreatePlatoon(companyId, platoonId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}