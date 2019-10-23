using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class SoldierController : Controller
    {
        private readonly ISoldierService _soldierService;

        public SoldierController(ISoldierService soldierService)
        {
            _soldierService = soldierService;
        }

        /// <summary>Get detailed information about the soldier</summary>
        /// <param name="soldierId">Soldier ID</param>
        /// <returns>Soldier details</returns>
        /// <response code="200">Soldier details</response>
        /// <response code="404">Soldier does not exist</response>
        [HttpGet("{soldierId}")]
        [ProducesResponseType(typeof(Zolnierz), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSoldierDetails([FromRoute] int soldierId)
        {
            var soldier = await _soldierService.GetSoldierInfoAsync(soldierId);

            if(soldier == null)
            {
                return NotFound();
            }

            return Ok(soldier);
        }
    }
}