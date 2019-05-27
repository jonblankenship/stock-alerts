using StockAlerts.Domain.Model;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface INotificationsService
    {
        Task SendNotificationsAsync(AppUser appUser, string subject, string message);
    }
}
