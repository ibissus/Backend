using KompaniaPchor.DTO_Models;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class FolderService : IFolderService
    {
        private readonly GenericRepo<Katalog> _folderRepo;
        private readonly IFilesService _fileService;
        private readonly GenericRepo<Plik> _fileRepo;

        public FolderService(GenericRepo<Katalog> folderRepo, IFilesService filesService,
            GenericRepo<Plik> fileRepo)
        {
            _folderRepo = folderRepo;
            _fileService = filesService;
            _fileRepo = fileRepo;
        }

        public async Task<List<Katalog>> GetCompanyFolder(int companyId, int? rootFolder = null)
        {
            return await _folderRepo.Get()
                .Where(f => f.NrKompanii == companyId && f.NrPlutonu == null && f.IdKataloguNadrzednego == rootFolder)
                .Include(f => f._KatalogiPodrzedne)
                .Include(f => f._Pliki)
                .ToListAsync();
        }

        public async Task<DTO_FolderContent> GetCompanyFolderContent(int companyId, int? rootFolder = null)
        {
            return new DTO_FolderContent
            {
                Folders = await GetCompanyFolder(companyId, rootFolder),
                Files = !rootFolder.HasValue ? null : await _fileRepo.Get().Where(f => f.IdKatalogu == rootFolder).ToListAsync()
            };
        }

        public async Task<DTO_FolderContent> GetOtherFolder(int companyId, int? platoonId, int? rootFolder = null)
        {
            return new DTO_FolderContent
            {
                Folders = await _folderRepo.Get()
                        .Where(f => f.NrKompanii == companyId && f.NrPlutonu == platoonId && f.IdKataloguNadrzednego == rootFolder)
                        .Include(f => f._KatalogiPodrzedne)
                        .Include(f => f._Pliki)
                        .ToListAsync(),
                Files = !rootFolder.HasValue ? null : await _fileRepo.Get().Where(f => f.IdKatalogu == rootFolder).ToListAsync()
            };

        }

        public async Task<Katalog> CreateFolder(int companyId, int? platoonId, string folderName, int? rootFolder = null)
        {
            var folder = new Katalog
            {
                IdKataloguNadrzednego = rootFolder,
                Nazwa = folderName,
                NrKompanii = companyId,
                NrPlutonu = platoonId
            };

            _folderRepo.Add(folder);
            await _folderRepo.SaveAsync();

            return folder;
        }

        public async Task RenameFolder(int folderId, string newName)
        {
            var folder = await _folderRepo.Get().Where(f => f.IdKatalogu == folderId).SingleOrDefaultAsync();
            folder.Nazwa = newName;

            _folderRepo.Update(folder);
            await _folderRepo.SaveAsync();
        }

        public async Task DeleteFolder(int folderId)
        {
            var folder = await _folderRepo.Get().Where(f => f.IdKatalogu == folderId)
                .Include(f => f._KatalogiPodrzedne).Include(f => f._Pliki)
                .SingleOrDefaultAsync();

            List<Task> tasks = new List<Task>();
            foreach(var file in folder._Pliki)
            {
                tasks.Add(Task.Run(() => _fileService.RemoveFileAsync(file.IdPliku, null)));
            }
            Task.WaitAll(tasks.ToArray());

            List<Task> tasks2 = new List<Task>();
            foreach (var subfolder in folder._KatalogiPodrzedne)
            {
                tasks2.Add(Task.Run(() => DeleteFolder(subfolder.IdKatalogu)));
            }
            Task.WaitAll(tasks2.ToArray());

            _folderRepo.Delete(folder);
            await _folderRepo.SaveAsync();
        }
    }
}
