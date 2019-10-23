using System;
using System.Collections.Generic;
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
    public class CompanyMembersController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyMembersController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>Get company soldiers</summary>
        /// <param name="companyId">Company ID number</param>
        /// <returns>List of soldiers</returns>
        /// <response code="200">Company soldiers</response>
        /// <response code="400">Error processing request</response>
        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(List<Zolnierz>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [GroupMember(GroupType.Company)]
        public async Task<IActionResult> GetAllCompanyMembers([FromRoute] int companyId)
        {
            try
            {
                return Ok(await _companyService.GetCompanyMembers(companyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Remove sorldier from company group</summary>
        /// <param name="companyId">Company ID number</param>
        /// <param name="soldierId">Soldier ID</param>
        /// <response code="200">Soldier removed</response>
        /// <response code="400">Error processing request</response>
        [HttpDelete("{companyId}")]
        [Authorize(Policy = CompanyCommanderPolicyName)]
        [GroupMember(GroupType.Company)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveSoldierFromCompany([FromRoute] int companyId, [FromQuery] int soldierId)
        {
            try
            {
                await _companyService.RemoveSoldierFromCompany(companyId, soldierId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}