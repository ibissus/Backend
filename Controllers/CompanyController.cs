using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.Filters;
using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Services;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        /// <summary>Get all comapny groups</summary>
        /// <returns>List of company groups</returns>
        ///<response code="200">Company list</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Kompania>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyList()
        {
            return Ok(await _companyService.GetCompanyList());
        }

        /// <summary>Get Company group details</summary>
        /// <param name="companyId">Company ID number</param>
        /// <returns>Company details</returns>
        /// <response code="200">Company details</response>
        /// <response code="400">Error processing request</response>
        /// <response code="404">Company does not exist</response>
        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(Kompania), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [GroupMember(GroupType.Company)]
        public async Task<IActionResult> GetCompany([FromRoute] int companyId)
        {
            Kompania company;

            try
            {
                company = await _companyService.GetCompanyDetails(companyId, User.IsInRole(CompanyCommanderRoleName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            if(company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        /// <summary>Create new company group</summary>
        /// <param name="companyId">Company number</param>
        /// <response code="200">Company created</response>
        /// <response code="400">Error processing request</response>
        [HttpPost("{companyId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCompany([FromRoute] int companyId)
        {
            try
            {
                await _companyService.CreateCompany(companyId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}