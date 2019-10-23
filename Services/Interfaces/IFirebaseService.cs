using KompaniaPchor.DTO_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KompaniaPchor.Services.Interfaces
{
    public interface IFirebaseService
    {
        Task SubscribeToCompanyTopic(int soldierId, int companyId, string token = null);
        Task SubscribeToPlatoonTopic(int soldierId, int companyId, int platoonId, bool assistant, string token = null);
        Task SendNotificationToTopic(DTO_NotificationForm form);
        Task UnsubscribeAllTopics(int soldierId, int? companyId, int? platoonId, bool assistant);
        Task UpdateUserToken(int soldierId, string oldToken, string newToken);
    }
}
