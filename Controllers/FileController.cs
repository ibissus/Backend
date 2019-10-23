using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Filters;
using KompaniaPchor.Services;
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
    public class FileController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly IFilesService _filesService;
        public FileController(IFilesService filesService, IHostingEnvironment env)
        {
            _env = env;
            _filesService = filesService;
        }

        /// <summary>Download file</summary>
        /// <param name="id">File ID</param>
        /// <returns>Physical File</returns>
        /// <response code="200">File</response>
        /// <response code="400">Download failed</response>
        /// <response code="400">File not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(File), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile([FromRoute] Guid id)
        {
            try
            {
                var file = await _filesService.GetFileAsync(id, User.Identity.Name);
                var path = Path.Combine(_env.WebRootPath, "files", id.ToString() + file.Rozszerzenie);

                Response.Headers.Add("OriginalFileName", WebUtility.UrlEncode(file.Opis));
                return PhysicalFile(path, file.Naglowek, id.ToString() + file.Rozszerzenie);
            }
            catch (NullReferenceException)
            {
                return NotFound("File does not exist");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid("You have no rights to read this file");
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>Upload new file</summary>
        /// <param name="form">Upload form</param>
        /// <response code="200">Upload success</response>
        /// <response code="200">Upload failed</response>
        [HttpPost, DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFileAsync([FromForm] DTO_FileUpload form)
        {
            try
            {
                await _filesService.AddFileAsync(form, User.Identity.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Remove file from server</summary>
        /// <param name="id">File ID</param>
        /// <response code="200">File deleted</response>
        /// <response code="400">Removing failed</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFileAsync([FromRoute] Guid id)
        {
            try
            {
                await _filesService.RemoveFileAsync(id, User.Identity.Name);
                return Ok();
            }
            catch (NullReferenceException)
            {
                return NotFound("File does not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}