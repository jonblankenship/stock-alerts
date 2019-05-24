using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockAlerts.Data.Model
{
    public class UserPreferences : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserPreferencesId { get; set; }

        public Guid AppUserId { get; set; }

        public bool ShouldSendEmail { get; set; }

        public bool ShouldSendPush { get; set; }

        public bool ShouldSendSms { get; set; }

        public string SmsNumber { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
