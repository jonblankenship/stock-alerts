using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StockAlerts.Data.Model
{
    public class AppUser : Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AppUserId { get; set; }

        public Guid UserId { get; set; }

        public virtual ICollection<AlertDefinition> AlertDefinitions { get; set; }

        public virtual UserPreferences UserPreferences { get; set; }
    }
}
