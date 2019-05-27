using System;

namespace StockAlerts.Domain.Model
{
    public class AppUser
    {
        public Guid AppUserId { get; set; }

        public Guid UserId { get; set; }

        public UserPreferences UserPreferences { get; set; }
    }
}
