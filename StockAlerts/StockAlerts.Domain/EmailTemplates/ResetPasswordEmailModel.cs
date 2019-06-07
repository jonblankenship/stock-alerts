using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.EmailTemplates
{
    public class ResetPasswordEmailModel : IEmailTemplateModel
    {
        public ResetPasswordEmailModel(string callbackUrl)
        {
            CallbackUrl = callbackUrl;
        }

        public string CallbackUrl { get; }

        public string Subject => "Reset your Stock Alerts password";

        public string TemplateName => "ResetPasswordEmailTemplate";
    }
}
