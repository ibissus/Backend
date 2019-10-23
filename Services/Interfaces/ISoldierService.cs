using KompaniaPchor.ORM_Models;
using System.Threading.Tasks;

namespace KompaniaPchor.Services.Interfaces
{
    public interface ISoldierService
    {
        Task<Zolnierz> GetSoldierInfoAsync(int? soldierId);
    }
}