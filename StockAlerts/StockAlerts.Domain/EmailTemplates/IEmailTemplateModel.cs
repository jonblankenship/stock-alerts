using System;
using System.Collections.Generic;
using System.Text;

namespace StockAlerts.Domain.EmailTemplates
{
    public interface IEmailTemplateModel
    {
        string Subject { get; }

        string TemplateName { get; }
    }
}
