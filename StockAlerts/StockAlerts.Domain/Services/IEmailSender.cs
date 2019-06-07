using StockAlerts.Domain.EmailTemplates;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toAddress, string subject, string message);

        Task SendEmailAsync(string toAddress, IEmailTemplateModel templateModel, bool shouldBccSupport = false);
    }
}
