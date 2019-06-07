using System;
using System.Collections.Generic;
using System.Text;
using StockAlerts.Domain.Model;

namespace StockAlerts.Domain.EmailTemplates
{
    public class WelcomeEmailModel : IEmailTemplateModel
    {
        private readonly AppUser _appUser;

        public WelcomeEmailModel(AppUser appUser)
        {
            _appUser = appUser;
        }

        public string Subject => "Welcome to Stock Alerts";

        public string TemplateName => "WelcomeEmailTemplate";

        public string UserName => _appUser.UserName;
    }
}
