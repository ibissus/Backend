using System.Collections.Generic;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.ORM_Models;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IFolderService
    {
        Task<List<Katalog>> GetCompanyFolder(int companyId, int? rootFolder = null);
        Task<DTO_FolderContent> GetOtherFolder(int companyId, int? platoonId, int? rootFolder = null);
        Task<Katalog> CreateFolder(int companyId, int? platoonId, string folderName, int? rootFolder = null);
        Task RenameFolder(int folderId, string newName);
        Task DeleteFolder(int folderId);
        Task<DTO_FolderContent> GetCompanyFolderContent(int companyId, int? rootFolder = null);
    }
}