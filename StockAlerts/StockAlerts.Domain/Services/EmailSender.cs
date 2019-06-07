using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.Core;
using StockAlerts.Domain.EmailTemplates;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly ISettings _settings;

        public EmailSender(IFluentEmail fluentEmail, ISettings settings)
        {
            _fluentEmail = fluentEmail ?? throw new ArgumentNullException(nameof(fluentEmail));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task SendEmailAsync(string toAddress, IEmailTemplateModel templateModel, bool shouldBccSupport = false)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var email = _fluentEmail
                .To(toAddress);

            if (shouldBccSupport)
                email.BCC(_settings.EmailSenderOptions.SupportEmailAddress);

            email.Subject(templateModel.Subject)
                .UsingTemplateFromEmbedded($"{assembly.GetName().Name}.EmailTemplates.{templateModel.TemplateName}.cshtml", templateModel, assembly);

            var response = await email.SendAsync();
        }

        public async Task SendEmailAsync(string toAddress, string subject, string message)
        {
            var email = _fluentEmail
                .To(toAddress)
                .Subject(subject)
                .Body(message);

            var response = await email.SendAsync();
        }
    }
}
