using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Services.Interfaces
{
    public class PlatoonService : IPlatoonService
    {
        private readonly GenericRepo<Pluton> _platoonRepo;
        private readonly GenericRepo<Prosba> _requestRepo;
        private readonly GenericRepo<Zolnierz> _soldierRepo;
        private readonly ICompanyService _companyService;
        private readonly IFirebaseService _firebaseService;
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleService _roleService;

        public PlatoonService(GenericRepo<Pluton> platoonRepo, GenericRepo<Prosba> requestRepo, IFirebaseService firebaseService,
            GenericRepo<Zolnierz> soldierRepo, UserManager<SystemUser> userManager, ICompanyService companyService, RoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
            _platoonRepo = platoonRepo;
            _requestRepo = requestRepo;
            _soldierRepo = soldierRepo;
            _companyService = companyService;
            _firebaseService = firebaseService;
        }

        /// <summary>Request a Platoon Commander Assistant Role</summary>
        public async Task ApplyForPCARole(int companyId, int platoonId, string requestingUser)
        {
            var user = await _userManager.FindByNameAsync(requestingUser);
            var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

            if (requesting.NrKompanii != companyId || requesting.NrPlutonu != platoonId)
            {
                throw new InvalidOperationException("User must be assigned to the company first and be a soldier of the platoon");
            }

            var request = new Prosba
            {
                NrKompanii = companyId,
                NrPlutonu = platoonId,
                IdZglaszajacego = requesting.IdOsoby,
                TypProsby = TypProsby.PA
            };

            _requestRepo.Add(request);
            await _requestRepo.SaveAsync();
        }

        public async Task JoinPlatoonGroup(int companyId, int platoonId, string requestingUser)
        {
            var user = await _userManager.FindByNameAsync(requestingUser);
            var soldier = await _soldierRepo.Get().AsNoTracking().Where(p => p.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

            if(soldier.NrKompanii != companyId)
            {
                throw new Exception("Soldier is not a member of this company");
            }

            var request = new Prosba
            {
                NrKompanii = companyId,
                NrPlutonu = platoonId,
                IdZglaszajacego = soldier.IdOsoby,
                TypProsby = TypProsby.JP
            };

            _requestRepo.Add(request);
            await _requestRepo.SaveAsync();
        }

        public async Task<List<Prosba>> GetPlatoonRequests(int companyId, int platoonId)
        {
            return await _requestRepo.Get().AsNoTracking().Where(r => r.NrKompanii == companyId && r.NrPlutonu == platoonId && r.Obsluzona == false).ToListAsync();
        }

        public async Task AcceptRequest(Prosba request, bool accepted)
        {
            if (accepted)
            {
                var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == request.IdZglaszajacego).SingleOrDefaultAsync();

                if (request.TypProsby == TypProsby.PA)
                {
                    if(soldier.NrPlutonu != request.NrPlutonu)
                    {
                        throw new Exception("Soldier is not a member of this platoon");
                    }

                    soldier.Funkcyjny = true;
                    _soldierRepo.Update(soldier);
                    await _soldierRepo.SaveAsync();

                    var user = (await _userManager.GetUsersInRoleAsync(UserRoleName)).Where(u => u.IdOsoby == request.IdZglaszajacego).SingleOrDefault();

                    try
                    {
                        await _roleService.AddToAssRole(user);
                        await _firebaseService.SubscribeToPlatoonTopic(soldier.IdOsoby, request.NrKompanii, (int)request.NrPlutonu, true);
                    }
                    catch (Exception) { }
                    
                }
                else
                {
                    soldier.NrPlutonu = request.NrPlutonu;
                    _soldierRepo.Update(soldier);
                    await _soldierRepo.SaveAsync();
                    try
                    {
                        await _firebaseService.SubscribeToPlatoonTopic(soldier.IdOsoby, request.NrKompanii, (int)request.NrPlutonu, false);
                    }
                    catch (Exception){ }
                    
                }
            }

            _requestRepo.Delete(request);
            await _requestRepo.SaveAsync();
        }

        public async Task<Pluton> GetPlatoonDetails(int companyId, int platoonId, bool platoonCommander)
        {
            if (platoonCommander)
            {
                return await _platoonRepo.Get().AsNoTracking().Where(c => c.NrKompanii == companyId && c.NrPlutonu == platoonId)
                 .Include(c => c._Kompania)
                 .Include(c => c._Zolnierze)
                 .Include(C => C._Dowodca)
                 .Include(c => c._Katalogi)
                 .Include(c => c._Prosby).ThenInclude(r => r._Zglaszajacy)
                 .SingleOrDefaultAsync();
            }
            else
            {
                return await _platoonRepo.Get().AsNoTracking().Where(c => c.NrKompanii == companyId && c.NrPlutonu == platoonId)
                 .Include(c => c._Kompania)
                 .Include(c => c._Zolnierze)
                 .Include(C => C._Dowodca)
                 .Include(c => c._Katalogi)
                 .SingleOrDefaultAsync();
            }
        }

        public async Task AssignNewPlatoonCommander(Prosba request, bool accepted)
        {
            if (accepted)
            {
                var soldier = await _soldierRepo.Get()
                    .Where(s => s.IdOsoby == request.IdZglaszajacego).SingleOrDefaultAsync();
                var platoon = await _platoonRepo.Get()
                    .Where(p => p.NrKompanii == request.NrKompanii && p.NrPlutonu == request.NrPlutonu)
                    .SingleOrDefaultAsync();

                soldier.NrPlutonu = request.NrPlutonu;
                platoon.IdDowodcy = soldier.IdOsoby;

                _soldierRepo.Update(soldier);
                await _soldierRepo.SaveAsync();

                _platoonRepo.Update(platoon);
                await _platoonRepo.SaveAsync();

                var user = (await _userManager.GetUsersInRoleAsync(UserRoleName)).Where(u => u.IdOsoby == request.IdZglaszajacego).SingleOrDefault();
                
                try
                {
                    await _roleService.AddToPCRole(user);
                    await _firebaseService.SubscribeToPlatoonTopic(request.IdZglaszajacego, request.NrKompanii,
                                        (int)request.NrPlutonu, false);
                }
                catch (Exception){ }
            }

            _requestRepo.Delete(request);
            await _requestRepo.SaveAsync();
        }

        public async Task<bool> IsUserAssignedToPlatoon(int companyId, int platoonId, string userName)
        {
            if (userName == "SuperUser")
            {
                return true;
            }

            var user = await _userManager.FindByNameAsync(userName);
            var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

            return soldier.NrKompanii == companyId && soldier.NrPlutonu == platoonId ? true : false;
        }

        public async Task<Pluton> CreatePlatoon(int companyId, int platoonId, string creatorName)
        {
            var user = await _userManager.FindByNameAsync(creatorName);

            if (user.IdOsoby == null)
            {
                throw new Exception("SuperUser cannot be a commander of a company");
            }

            if (await _companyService.GetCompanyDetails(companyId, false) == null)
            {
                throw new NullReferenceException("Company does not exist");
            }

            if (! await _companyService.IsUserAssignedToCompany(companyId, creatorName))
            {
                throw new InvalidOperationException("User must be assigned to the company first");
            }

            var platoon = new Pluton
            {
                NrKompanii = companyId,
                NrPlutonu = platoonId,
                //IdDowodcy = (int)user.IdOsoby
            };

            _platoonRepo.Add(platoon);
            await _platoonRepo.SaveAsync();

            return platoon;
        }

        public virtual async Task<List<Pluton>> GetPlatoonList(int companyId)
        {
            return await _platoonRepo.Get().Where(c => c.NrKompanii == companyId)
                .OrderBy(p => p.NrPlutonu).AsNoTracking().ToListAsync();
        }

        public async Task<List<Zolnierz>> GetPlatoonMembers(int companyId, int platoonId)
        {
            return await _soldierRepo.Get().Where(c => c.NrKompanii == companyId && c.NrPlutonu == platoonId)
                .OrderBy(s => s.Nazwisko).ThenBy(s => s.Imie).ToListAsync();
        }
    }
}
