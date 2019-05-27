using StockAlerts.Domain.Model;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using StockAlerts.Domain.Factories;
using StockAlerts.Domain.QueueMessages;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly IQueueClient _emailNotificationQueueClient;
        private readonly IQueueClient _pushNotificationQueueClient;
        private readonly IQueueClient _smsNotificationQueueClient;

        public NotificationsService(
            IQueueClientFactory queueClientFactory,
            ISettings settings)
        {
            if (queueClientFactory == null) throw new ArgumentNullException(nameof(queueClientFactory));
            if (settings == null) throw new ArgumentNullException(nameof(settings));

            _emailNotificationQueueClient = queueClientFactory.CreateClient(settings.ServiceBusSettings.EmailNotificationQueue);
            _pushNotificationQueueClient = queueClientFactory.CreateClient(settings.ServiceBusSettings.PushNotificationQueue);
            _smsNotificationQueueClient = queueClientFactory.CreateClient(settings.ServiceBusSettings.SmsNotificationQueue);
        }

        public async Task SendNotificationsAsync(AppUser appUser, string subject, string message)
        {
            // Queue appropriate notification messages
            if (appUser.UserPreferences.ShouldSendEmail)
            {
                await SendEmailNotificationAsync(appUser, subject, message);
            }
            if (appUser.UserPreferences.ShouldSendPush)
            {
                await SendPushNotificationAsync(appUser, subject, message);
            }
            if (appUser.UserPreferences.ShouldSendSms)
            {
                await SendSmsNotificationAsync(appUser, message);
            }
        }

        private async Task SendEmailNotificationAsync(AppUser appUser, string subject, string message)
        {
            var queueMessageBody = new EmailNotificationMessage
            {
                EmailAddress = appUser.UserPreferences.EmailAddress,
                Subject = subject,
                Message = message
            };

            var queueMessageBodyJson = JsonConvert.SerializeObject(queueMessageBody);
            var queueMessage = new Message(Encoding.UTF8.GetBytes(queueMessageBodyJson));
            await _emailNotificationQueueClient.SendAsync(queueMessage);
        }

        private async Task SendPushNotificationAsync(AppUser appUser, string subject, string message)
        {
            var queueMessageBody = new PushNotifcationMessage
            {
                AppUserId = appUser.AppUserId,
                Subject = subject,
                Message = message
            };

            var queueMessageBodyJson = JsonConvert.SerializeObject(queueMessageBody);
            var queueMessage = new Message(Encoding.UTF8.GetBytes(queueMessageBodyJson));
            await _pushNotificationQueueClient.SendAsync(queueMessage);
        }

        private async Task SendSmsNotificationAsync(AppUser appUser, string message)
        {
            var queueMessageBody = new SmsNotificationMessage
            {
                PhoneNumber = appUser.UserPreferences.SmsNumber,
                Message = message
            };

            var queueMessageBodyJson = JsonConvert.SerializeObject(queueMessageBody);
            var queueMessage = new Message(Encoding.UTF8.GetBytes(queueMessageBodyJson));
            await _pushNotificationQueueClient.SendAsync(queueMessage);
        }
    }
}
