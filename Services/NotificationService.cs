using KompaniaPchor.DTO_Models;
using KompaniaPchor.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KompaniaPchor.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;

        public NotificationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendNotificationAsync(DTO_NotificationForm form)
        {
            var serverKey = "key=" + _configuration["FirebaseConfig:serverKey"];
            var senderId = "id=" + _configuration["FirebaseConfig:senderId"];

            var data = new
            {
                //To = form.,
                notification = new { Title = form.Title, Body = form.Body }
            };

            var jsonBody = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
            {
                httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
                httpRequest.Headers.TryAddWithoutValidation("Sender", senderId);
                httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var result = await httpClient.SendAsync(httpRequest);

                    return result.IsSuccessStatusCode; 
                }
            }
        }
    }
}
