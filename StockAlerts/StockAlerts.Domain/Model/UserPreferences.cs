using System;

namespace StockAlerts.Domain.Model
{
    public class UserPreferences
    {
        public Guid UserPreferencesId { get; set; }

        public Guid AppUserId { get; set; }

        public bool ShouldSendEmail { get; set; }

        public string EmailAddress { get; set; }

        public bool ShouldSendPush { get; set; }

        public bool ShouldSendSms { get; set; }

        public string SmsNumber { get; set; }
    }
}
