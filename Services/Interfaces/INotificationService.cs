using KompaniaPchor.DTO_Models;
using System.Threading.Tasks;

namespace KompaniaPchor.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendNotificationAsync(DTO_NotificationForm form);
    }
}