using StockAlerts.Domain.Model;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Services
{
    public interface IUserPreferencesService
    {
        Task<UserPreferences> GetUserPreferencesAsync(Guid appUserId);
    }
}
