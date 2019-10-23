using KompaniaPchor.Identity;
using KompaniaPchor.ORM_Models;
using KompaniaPchor.Repositories;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly GenericRepo<Kompania> _companyRepo;
        private readonly GenericRepo<Prosba> _requestRepo;
        private readonly GenericRepo<Zolnierz> _soldierRepo;
        private readonly IFirebaseService _firebaseService;
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleService _roleService;

        public CompanyService(GenericRepo<Kompania> companyRepo, GenericRepo<Prosba> requestRepo, RoleService roleService,
            GenericRepo<Zolnierz> soldierRepo, UserManager<SystemUser> userManager, IFirebaseService firebaseService)
        {
            _userManager = userManager;
            _roleService = roleService;
            _companyRepo = companyRepo;
            _requestRepo = requestRepo;
            _soldierRepo = soldierRepo;
            _firebaseService = firebaseService;
        }


        /// <summary>Request a Platoon Commander Role</summary>
        public async Task ApplyForPCRole(int companyId, int platoonId, string requestingUser)
        {
            var user = await _userManager.FindByNameAsync(requestingUser);
            var requesting = await _soldierRepo.Get().AsNoTracking().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

            if(requesting.NrKompanii != companyId)
            {
                throw new InvalidOperationException("User must be assigned to the company first");
            }

            var request = new Prosba
            {
                NrKompanii = companyId,
                NrPlutonu = platoonId,
                IdZglaszajacego = requesting.IdOsoby,
                TypProsby = TypProsby.PC
            };

            _requestRepo.Add(request);
            await _requestRepo.SaveAsync();
        }

        public async Task JoinCompannyGroup(int companyId, string requestingUser)
        {
            var user = await _userManager.FindByNameAsync(requestingUser);

            var request = new Prosba
            {
                NrKompanii = companyId,
                IdZglaszajacego = (int)user.IdOsoby,
                TypProsby = TypProsby.JC
            };

            _requestRepo.Add(request);
            await _requestRepo.SaveAsync();
        }

        public async Task<List<Prosba>> GetCompanyRequests(int companyId)
        {
            return await _requestRepo.Get().AsNoTracking().Where(r => r.NrKompanii == companyId && r.Obsluzona == false).ToListAsync();
        }

        public async Task AcceptRequest(Prosba request, bool accepted)
        {
            if (accepted)
            {
                var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == request.IdZglaszajacego).SingleOrDefaultAsync();
                soldier.NrKompanii = request.NrKompanii;
                _soldierRepo.Update(soldier);
                await _soldierRepo.SaveAsync();
                try
                {
                    await _firebaseService.SubscribeToCompanyTopic(soldier.IdOsoby, request.NrKompanii);
                }
                catch (Exception){ }
                
            }

            _requestRepo.Delete(request);
            await _requestRepo.SaveAsync();
        }

        public async Task RemoveSoldierFromCompany(int companyId, int soldierId)
        {
            var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == soldierId).SingleOrDefaultAsync();

            if(soldier == null)
            {
                throw new Exception("Soldier does not exist");
            }

            soldier.NrPlutonu = null;
            soldier.NrKompanii = null;

            _soldierRepo.Update(soldier);
            await _soldierRepo.SaveAsync();

            try
            {
                var user = (await _userManager.GetUsersInRoleAsync(UserRoleName)).Where(u => u.IdOsoby == soldierId).SingleOrDefault();
                await _userManager.RemoveFromRoleAsync(user, AssistantRoleName);
                await _userManager.RemoveFromRoleAsync(user, PlatoonCommanderRoleName);
                await _firebaseService.UnsubscribeAllTopics(soldier.IdOsoby, (int)soldier.NrKompanii, soldier.NrPlutonu, soldier.Funkcyjny);
            }
            catch (Exception){ }
            
        }

        public async Task<Kompania> GetCompanyDetails(int companyId, bool companyCommander)
        {
            if (companyCommander)
            {
                return await _companyRepo.Get().AsNoTracking().Where(c => c.NrKompanii == companyId)
                .Include(c => c._Plutony)
                .Include(c => c._Dowodca)
                .Include(c => c._Zolnierze)
                .Include(c => c._PlanZajec)
                .Include(c => c._Prosby).ThenInclude(r => r._Zglaszajacy)
                .SingleOrDefaultAsync();
            }
            else
            {
                return await _companyRepo.Get().AsNoTracking().Where(c => c.NrKompanii == companyId)
                .Include(c => c._Plutony)
                .Include(c => c._Dowodca)
                .Include(c => c._Zolnierze)
                .Include(c => c._PlanZajec)
                .SingleOrDefaultAsync();
            }
        }

        public async Task<bool> IsUserAssignedToCompany(int companyId, string userName)
        {
            if(userName == "SuperUser")
            {
                return true;
            }

            var user = await _userManager.FindByNameAsync(userName);
            var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();

            return soldier.NrKompanii == companyId ? true : false;
        }

        public async Task<Kompania> CreateCompany(int companyId, string creatorName)
        {
            var user = await _userManager.FindByNameAsync(creatorName);

            if (user.IdOsoby == null)
            {
                throw new Exception("SuperUser cannot be a commander of a company");
            }

            var company = new Kompania
            {
                NrKompanii = companyId,
                IdDowodcy = (int)user.IdOsoby
            };

            var soldier = await _soldierRepo.Get().Where(s => s.IdOsoby == user.IdOsoby).SingleOrDefaultAsync();
            soldier.NrKompanii = companyId;

            _companyRepo.Add(company);
            await _companyRepo.SaveAsync();

            _soldierRepo.Update(soldier);
            await _soldierRepo.SaveAsync();

            await _roleService.AddToCCRole(user);

            return company;
        }

        public async Task<List<Kompania>> GetCompanyList()
        {
            return await _companyRepo.Get().OrderBy(c => c.NrKompanii).AsNoTracking().ToListAsync();
        }

        public async Task<List<Zolnierz>> GetCompanyMembers(int companyId)
        {
            return await _soldierRepo.Get().Where(c => c.NrKompanii == companyId).ToListAsync();
        }
    }
}
