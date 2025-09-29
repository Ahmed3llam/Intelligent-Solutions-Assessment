using Application.Common.Interfaces.EventInterfaces;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notifications
{
    public class FirebaseNotification : INotification
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FirebaseNotification> _logger;
        //private readonly string _fcmSendUrl = "fcm/send"; 
        //private readonly string _serverKey;
        private readonly string _projectId;
        private readonly GoogleCredential _googleCredential;
        public FirebaseNotification(HttpClient httpClient, ILogger<FirebaseNotification> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;

            _projectId = configuration["NotificationSettings:ProjectId"]
                ?? throw new InvalidOperationException("NotificationSettings:ProjectId not configured.");

            var serviceAccountPath = configuration["NotificationSettings:ServiceAccountPath"]
                ?? throw new InvalidOperationException("NotificationSettings:ServiceAccountPath not configured.");

            _googleCredential = GoogleCredential.FromFile(serviceAccountPath)
                .CreateScoped("https://www.googleapis.com/auth/firebase.messaging");
        }

        public async Task SendNotificationAsync(string topic, string title, string body)
        {
            _logger.LogInformation("Attempting to send Firebase Push to topic: {Topic}", topic);

            var token = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var pushPayload = new
            {
                message = new
                {
                    topic = topic,
                    notification = new
                    {
                        title = title,
                        body = body
                    }
                }
            };

            var url = $"https://fcm.googleapis.com/v1/projects/{_projectId}/messages:send";
            var response = await _httpClient.PostAsJsonAsync(url, pushPayload);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully dispatched notification to topic {Topic}. Response Status: {Status}", topic, response.StatusCode);
            }
            else
            {
                _logger.LogError("Failed to dispatch notification to topic {Topic}. Status: {Status}", topic, response.StatusCode);
            }
        }
    }
}
