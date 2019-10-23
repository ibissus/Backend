using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using KompaniaPchor.DTO_Models;
using KompaniaPchor.Identity;
using KompaniaPchor.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KompaniaPchor.Identity.IdentityConfiguration;

namespace KompaniaPchor.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<SystemUser> _userManager;
        private readonly ISoldierService _soldierService;
        private readonly FirebaseApp _app;
        private readonly FirebaseAuth _auth;
        public FirebaseService(IConfiguration configuration, UserManager<SystemUser> userManager,
            IHttpContextAccessor httpContextAccessor, ISoldierService soldierService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _soldierService = soldierService;

            _app = FirebaseApp.DefaultInstance ?? FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("company-81598-firebase-adminsdk-ihfsh-9c2003e91b.json"),
            });
            _auth = FirebaseAuth.DefaultInstance;
        }

        public async Task SubscribeToCompanyTopic(int soldierId, int companyId, string token = null)
        {
            if (token == null) GetUserToken(soldierId, out token);

            var registrationTokens = new List<string>() { token };
            var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, PrepareTopic(companyId));
        }

        public async Task SubscribeToPlatoonTopic(int soldierId, int companyId, int platoonId, bool assistant, string token = null)
        {
            if (token == null) GetUserToken(soldierId, out token);

            var registrationTokens = new List<string>() { token };
            await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, PrepareTopic(companyId, platoonId, false));

            if (assistant)
            {
                await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, PrepareTopic(companyId, platoonId, assistant));
            }
        }

        public async Task UnsubscribeAllTopics(int soldierId, int? companyId, int? platoonId, bool assistant)
        {
            GetUserToken(soldierId, out string token);

            if (token == null) return;

            var registrationTokens = new List<string>() { token };

            if (companyId != null)
            {
                if (assistant)
                {
                    await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(registrationTokens, PrepareTopic((int)companyId, platoonId, assistant));
                }
                if (platoonId != null && platoonId > 0)
                {
                    await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(registrationTokens, PrepareTopic((int)companyId, platoonId, false));
                }

                await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(registrationTokens, PrepareTopic((int)companyId, null, false));
            }
            
        }

        public  async Task UpdateUserToken(int soldierId, string oldToken, string newToken)
        {
            var soldier = await _soldierService.GetSoldierInfoAsync(soldierId);

            if(soldier != null)
            {
                await UnsubscribeAllTopics(soldier.IdOsoby, soldier.NrKompanii, soldier.NrPlutonu, soldier.Funkcyjny);

                if(soldier.NrKompanii != null)
                {
                    await SubscribeToCompanyTopic(soldierId, (int)soldier.NrKompanii, newToken);
                    if (soldier.NrPlutonu != null && soldier.NrPlutonu > 0)
                    {
                        await SubscribeToPlatoonTopic(soldierId, (int)soldier.NrKompanii, (int)soldier.NrPlutonu, false, newToken);
                        if (soldier.Funkcyjny)
                        {
                            await SubscribeToPlatoonTopic(soldierId, (int)soldier.NrKompanii, (int)soldier.NrPlutonu, true, newToken);
                        }
                    }
                }
                
            }
        }


        private string PrepareTopic(int companyId, int? platoonId = null, bool assistant = false)
        {
            var topic = new StringBuilder();

            topic.Append("cmp-");
            topic.Append(companyId);

            if(platoonId != null && platoonId > 0)
            {
                topic.Append("pl-");
                topic.Append((int)platoonId);
            }

            if (assistant)
            {
                topic.Append("-ass");
            }

            return topic.ToString();
        }

        private void GetUserToken(int soldierId, out string token)
        {
            /*var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var token = user.FirebaseToken;*/

            var users = _userManager.GetUsersInRoleAsync(UserRoleName).GetAwaiter().GetResult();
            token = users.Where(u => u.IdOsoby == soldierId).Select(u => u.FirebaseToken).SingleOrDefault();
        }

        public async Task SendNotificationToTopic(DTO_NotificationForm form)
        {
            var topic = PrepareTopic(form.CompanyId, form.PlatoonId, form.OnlyAssistants);

            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = form.Title,
                    Body = form.Body
                },
                Topic = topic,
            };

            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
