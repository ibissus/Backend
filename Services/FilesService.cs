using KompaniaPchor.DTO_Models;
using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class FilesService : IFilesService
    {
        private readonly GenericRepo<Plik> _filesRepo;
        private readonly GenericRepo<Katalog> _folderRepo;
        private readonly GenericRepo<Zolnierz> _soldierRepo;
        private readonly GenericRepo<Kompania> _companyRepo;
        private readonly IHostingEnvironment _env;
        private readonly UserManager<SystemUser> _userManager;
        public FilesService(GenericRepo<Plik> filesRepo, GenericRepo<Zolnierz> soldierRepo, GenericRepo<Katalog> folderRepo,
            IHostingEnvironment env, UserManager<SystemUser> userManager, GenericRepo<Kompania> companyRepo)
        {
            _env = env;
            _userManager = userManager;
            _filesRepo = filesRepo;
            _folderRepo = folderRepo;
            _soldierRepo = soldierRepo;
            _companyRepo = companyRepo;
        }

        public async Task AddFileAsync(DTO_FileUpload uploadForm, string requestingUser)
        {
            var guid = Guid.NewGuid();
            var ext = Path.GetExtension(uploadForm.File.FileName);

            var file = new Plik
            {
                IdPliku = guid,
                IdKatalogu = uploadForm.ContentFolder,
                Rozszerzenie = ext,
                Naglowek = uploadForm.File.ContentType,
                Opis = uploadForm.Description,
                Dodano = DateTime.Now
            };

            var folder = await _folderRepo.Get().Where(f => f.IdKatalogu == uploadForm.ContentFolder).SingleOrDefaultAsync();

            var user = await _userManager.FindByNameAsync(requestingUser);

            if(user.UserName.ToLower() != "superuser")
            {
                var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

                if (requesting.NrKompanii != folder.NrKompanii)
                {
                    throw new UnauthorizedAccessException("You must be a member of requested comapny");
                }
                else if (folder.NrPlutonu != null && requesting.NrPlutonu != folder.NrPlutonu)
                {
                    throw new UnauthorizedAccessException("You must be a member of requested platoon");
                }
            }
            
            var uploadPath = Path.Combine(_env.WebRootPath, "files", guid.ToString() + ext);
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                await uploadForm.File.CopyToAsync(fileStream);
            }

            _filesRepo.Add(file);
            await _filesRepo.SaveAsync();
        }

        public async Task AddTimeTableFile(int companyId, IFormFile File, string requestingUser)
        {
            var guid = Guid.NewGuid();
            var ext = Path.GetExtension(File.FileName);

            var file = new Plik
            {
                IdPliku = guid,
                IdKatalogu = null,
                Opis = "Plan Zajęć",
                NrKompanii = companyId,
                Rozszerzenie = ext,
                Naglowek = File.ContentType,
                Dodano = DateTime.Now,
            };

            var user = await _userManager.FindByNameAsync(requestingUser);

            if (user.UserName.ToLower() != "superuser")
            {
                var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

                if (requesting.NrKompanii != companyId)
                {
                    throw new UnauthorizedAccessException("You must be a member of requested comapny");
                }
            }

            var oldTimeTable = await _companyRepo.Get().Where(c => c.NrKompanii == companyId)
                                        .Include(c => c._PlanZajec)
                                        .Select(c => c._PlanZajec)
                                        .SingleOrDefaultAsync();

            if(oldTimeTable != null)
            {
                await RemoveFileAsync(oldTimeTable.IdPliku, requestingUser);
            }
            
            var uploadPath = Path.Combine(_env.WebRootPath, "files", guid.ToString() + ext);
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                await File.CopyToAsync(fileStream);
            }

            _filesRepo.Add(file);
            await _filesRepo.SaveAsync();
        }

        public async Task<Plik> GetTimeTableFile(int companyId)
        {
            return await _companyRepo.Get().Where(c => c.NrKompanii == companyId)
                    .Include(c => c._PlanZajec).Select(c => c._PlanZajec)
                    .SingleOrDefaultAsync();
        }

        public async Task<Plik> GetFileAsync(Guid id, string requestingUser)
        {
            var file = await _filesRepo.Get().Where(p => p.IdPliku == id).Include(f => f._Katalog).SingleOrDefaultAsync();

            var user = await _userManager.FindByNameAsync(requestingUser);

            if(user.UserName.ToLower() != "superuser")
            {
                var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

                if (file._Katalog != null)
                {
                    if (requesting.NrKompanii != file._Katalog.NrKompanii)
                    {
                        throw new UnauthorizedAccessException("You must be a member of requested comapny");
                    }
                    else if (file._Katalog.NrPlutonu != null && requesting.NrPlutonu != file._Katalog.NrPlutonu)
                    {
                        throw new UnauthorizedAccessException("You must be a member of requested platoon");
                    }
                }
            }
            
            if (file == null)
            {
                throw new NullReferenceException();
            }

            return file;
        }

        public async Task RemoveFileAsync(Guid id, string requestingUser)
        {
            var file = await _filesRepo.Get().Where(p => p.IdPliku == id).SingleOrDefaultAsync();

            if(requestingUser != null)
            {
                var user = await _userManager.FindByNameAsync(requestingUser);
                var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

                if(file._Katalog != null)
                {
                    if (requesting.NrKompanii != file._Katalog.NrKompanii)
                    {
                        throw new UnauthorizedAccessException("You must be a member of requested comapny");
                    }
                    else if (file._Katalog.NrPlutonu != null && requesting.NrPlutonu != file._Katalog.NrPlutonu)
                    {
                        throw new UnauthorizedAccessException("You must be a member of requested platoon");
                    }
                }

                if (file == null)
                {
                    throw new NullReferenceException();
                }
            }
            
            var path = Path.Combine(_env.WebRootPath, "files", id.ToString() + file.Rozszerzenie);

            File.Delete(path);
            _filesRepo.Delete(file);
            await _filesRepo.SaveAsync();
        }
    }
}
