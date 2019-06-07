using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.Settings
{
    public class EmailSenderOptions
    {
        public string MailGunApiKey { get; set; }

        public string Domain { get; set; }

        public string SupportEmailAddress { get; set; }
    }
}
