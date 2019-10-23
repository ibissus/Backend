using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Filters;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KompaniaPchor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize]
    public class TimeTableController : Controller
    {
        private readonly IFilesService _filesService;
        private readonly IHostingEnvironment _env;

        public TimeTableController(IFilesService filesService, IHostingEnvironment env)
        {
            _filesService = filesService;
            _env = env;
        }

        /// <summary>Get company timetable</summary>
        /// <param name="companyId">company ID</param>
        /// <returns>Image of timetable</returns>
        /// <response code="200">Image of company timetable</response>
        /// <response code="400">Downloading timetable failed</response>
        [HttpGet("{companyId}")]
        [GroupMember(GroupType.Company)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompanyTimeTable([FromRoute] int companyId)
        {
            try
            {
                var file = await _filesService.GetTimeTableFile(companyId);
                var path = Path.Combine(_env.WebRootPath, "files", file.IdPliku.ToString() + file.Rozszerzenie);

                Response.Headers.Add("OriginalFileName", WebUtility.UrlEncode(file.Opis));
                return PhysicalFile(path, file.Naglowek, file.IdPliku.ToString() + file.Rozszerzenie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Send new company's timetable</summary>
        /// <param name="companyId">company ID</param>
        /// <param name="dto">Timetable image</param>
        /// <response code="200">Timetable updated</response>
        /// <response code="400">Updating timetable failed</response>
        [HttpPost("{companyId}"), DisableRequestSizeLimit]
        [GroupMember(GroupType.Company)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompanyTimeTable([FromRoute] int companyId, [FromForm] DTO_TimeTable dto)
        {
            try
            {
                await _filesService.AddTimeTableFile(companyId, dto.File, User.Identity.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}