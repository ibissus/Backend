using KompaniaPchor.ORM_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<Kompania> GetCompanyDetails(int companyId, bool companyCommander);
        Task<List<Kompania>> GetCompanyList();
        Task<Kompania> CreateCompany(int companyId, string creatorName);
        Task ApplyForPCRole(int companyId, int platoonId, string requestingUser);
        Task<List<Prosba>> GetCompanyRequests(int companyId);
        Task AcceptRequest(Prosba request, bool accepted);
        Task JoinCompannyGroup(int companyId, string requestingUser);
        Task RemoveSoldierFromCompany(int companyId, int soldierId);
        Task<bool> IsUserAssignedToCompany(int companyId, string userName);
        Task<List<Zolnierz>> GetCompanyMembers(int companyId);
    }
}
