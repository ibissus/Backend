using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Filters;
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
    public class FolderController : Controller
    {
        private readonly IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        /// <summary>Get company group subfolders and files</summary>
        /// <param name="companyId">company ID</param>
        /// <param name="rootFolder">containing folder if any</param>
        /// <returns>Subfolders and files</returns>
        /// <response code="200">Folder tree</response>
        /// <response code="404">Non-existing folder</response>
        [HttpGet("{companyId}")]
        [GroupMember(GroupType.Company)]
        [ProducesResponseType(typeof(DTO_FolderContent), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyFolders([FromRoute] int companyId, [FromQuery] int? rootFolder = null)
        {
            var folder = await _folderService.GetCompanyFolderContent(companyId, rootFolder);

            if (folder == null)
            {
                return NotFound("Folder does not exist");
            }

            return Ok(folder);
        }

        /// <summary>Get platoon folders</summary>
        /// <param name="companyId">company ID</param>
        /// <param name="platoonId">platoon ID</param>
        /// <param name="rootFolder">containing folder if any</param>
        /// <returns>Folder tree</returns>
        /// <response code="200">Folder tree</response>
        /// <response code="404">Non-existing folder</response>
        [HttpGet]
        [GroupMember(GroupType.Company)]
        [ProducesResponseType(typeof(List<Katalog>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
#pragma warning disable CS1573
        public async Task<IActionResult> GetOtherFolders([FromServices] IPlatoonService platoonService, [FromQuery, Required] int companyId, [FromQuery, Required] int? platoonId = null, [FromQuery] int? rootFolder = null)
        {
            if (platoonId != null)
            {
                if (!await platoonService.IsUserAssignedToPlatoon(companyId, (int)platoonId, User.Identity.Name))
                {
                    return Forbid("You must be platoon member");
                }
            }

            var folder = await _folderService.GetOtherFolder(companyId, platoonId, rootFolder);

            if (folder == null)
            {
                return NotFound("Folder does not exist");
            }

            return Ok(folder);
        }

        /// <summary>Create new folder</summary>
        /// <param name="form">Create folder form</param>
        /// <response code="200">Folder created</response>
        /// <response code="400">Creating folder failed</response>
        [HttpPost]
        [ProducesResponseType(typeof(Katalog), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
#pragma warning disable CS1573
        public async Task<IActionResult> CreateFolder([FromServices] ICompanyService companyService, [FromServices] IPlatoonService platoonService, [FromBody] DTO_CreateFolder form)
        {
            if(string.IsNullOrEmpty(form.Name))
            {
                return BadRequest("Folder name is required");
            }

            if(! await companyService.IsUserAssignedToCompany(form.CompanyId, User.Identity.Name))
            {
                return Forbid("You must be company member");
            }

            if(form.PlatoonId != null)
            {
                if(! await platoonService.IsUserAssignedToPlatoon(form.CompanyId, (int)form.PlatoonId, User.Identity.Name))
                {
                    return Forbid("You must be platoon member");
                }
            }

            Katalog folder;
            try
            {
                folder = await _folderService.CreateFolder(form.CompanyId, form.PlatoonId, form.Name, form.RootFolderId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            return Ok(folder);
        }

        /// <summary>Rename folder</summary>
        /// <param name="folderId">Folder ID</param>
        /// <param name="newName">New name for a folder</param>
        /// <response code="200">Folder renamed</response>
        /// <response code="400">Renaming failed</response>
        [HttpPatch("{folderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFolderName([FromRoute] int folderId, [FromQuery, Required] string newName)
        {
            if (string.IsNullOrEmpty(newName))
            {
                return BadRequest("New name for a folder is required");
            }

            try
            {
                await _folderService.RenameFolder(folderId, newName);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>Delte folder</summary>
        /// <param name="folderId">Folder ID</param>
        /// <response code="200">Folder deleted</response>
        /// <response code="400">Deleting failed</response>
        [HttpDelete("{folderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteFolder([FromRoute] int folderId)
        {
            try
            {
                await _folderService.DeleteFolder(folderId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}