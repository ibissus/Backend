using System;
using System.Threading.Tasks;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.ORM_Models;
using Microsoft.AspNetCore.Http;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IFilesService
    {
        Task AddFileAsync(DTO_FileUpload uploadForm, string requestingUser);
        Task<Plik> GetFileAsync(Guid id, string requestingUser);
        Task RemoveFileAsync(Guid id, string requestingUser);
        Task AddTimeTableFile(int companyId, IFormFile File, string requestingUser);
        Task<Plik> GetTimeTableFile(int companyId);
    }
}