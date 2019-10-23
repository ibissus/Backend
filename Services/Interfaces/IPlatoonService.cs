using System.Collections.Generic;
using System.Threading.Tasks;
using KompaniaPchor.ORM_Models;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IPlatoonService
    {
        Task AcceptRequest(Prosba request, bool accepted);
        Task ApplyForPCARole(int companyId, int platoonId, string requestingUser);
        Task<Pluton> CreatePlatoon(int companyId, int platoonId, string creatorName);
        Task<Pluton> GetPlatoonDetails(int companyId, int platoonId, bool platoonCommander);
        Task<List<Pluton>> GetPlatoonList(int companyId);
        Task<List<Zolnierz>> GetPlatoonMembers(int companyId, int platoonId);
        Task<List<Prosba>> GetPlatoonRequests(int companyId, int platoonId);
        Task<bool> IsUserAssignedToPlatoon(int companyId, int platoonId, string userName);
        Task JoinPlatoonGroup(int companyId, int platoonId, string requestingUser);
        Task AssignNewPlatoonCommander(Prosba request, bool accepted);
    }
}