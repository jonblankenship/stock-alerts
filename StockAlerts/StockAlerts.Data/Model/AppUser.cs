using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace StockAlerts.Data.Model
{
    public class AppUser : Entity
    {
        public Guid AppUserId { get; set; }

        public Guid UserId { get; set; }

        public bool HasBeenGrantedAccess { get; set; }

        public virtual ICollection<AlertDefinition> AlertDefinitions { get; set; }

        public virtual UserPreferences UserPreferences { get; set; }

        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>();
        public List<RefreshToken> RefreshTokens { get; private set; }
    }
}
