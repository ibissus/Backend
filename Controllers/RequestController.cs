using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.ORM_Models;
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
    public class RequestController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IPlatoonService _platoonService;
        private readonly IRequestService _requestService;

        public RequestController(ICompanyService companyService, IPlatoonService platoonService, IRequestService requestService)
        {
            _companyService = companyService;
            _platoonService = platoonService;
            _requestService = requestService;
        }

        /// <summary>Accept or reject a request</summary>
        /// <param name="requestId">Request ID</param>
        /// <param name="decision">Accept or Reject [T / F]</param>
        [HttpPatch("{requestId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptRequest([FromRoute] int requestId, [Required, FromQuery] bool decision)
        {
            try
            {
                await _requestService.AcceptRequest(requestId, decision);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }

        /// <summary>Send a new request</summary>
        /// <param name="request">Request from</param>
        /// <remarks>
        /// RequestType:
        /// 1 - Platoon Commander
        /// 2 - Platoon Commander Assistant
        /// 3 - Join Company Group
        /// 4 - Join Platoon Group
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendRequest([FromBody] DTO_Request request)
        {
            try
            {
                switch (request.RequestType)
                {
                    /*case TypProsby.CC:
                        break;*/
                    case TypProsby.JC:
                        await _companyService.JoinCompannyGroup(request.CompanyId, User.Identity.Name);
                        break;
                    case TypProsby.JP:
                        await _platoonService.JoinPlatoonGroup(request.CompanyId, (int)request.PlatoonId, User.Identity.Name);
                        break;
                    case TypProsby.PA:
                        await _platoonService.ApplyForPCARole(request.CompanyId, (int)request.PlatoonId, User.Identity.Name);
                        break;
                    case TypProsby.PC:
                        await _companyService.ApplyForPCRole(request.CompanyId, (int)request.PlatoonId, User.Identity.Name);
                        break;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok();
        }
    }
}