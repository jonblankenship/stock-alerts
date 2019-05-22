using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

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

        public AppUser AppUser { get; set; }
    }
}
