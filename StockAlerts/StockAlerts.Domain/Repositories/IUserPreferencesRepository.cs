using StockAlerts.Domain.Model;
using System;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Repositories
{
    public interface IUserPreferencesRepository
    {
        /// <summary>
        /// Retrieves the <see cref="UserPreferences"/> for the given <see cref="appUserId"/>
        /// </summary>
        /// <param name="appUserId"></param>
        /// <returns></returns>
        Task<UserPreferences> GetUserPreferencesAsync(Guid appUserId);

        /// <summary>
        /// Persists <see cref="userPreferences"/> to the database
        /// </summary>
        /// <param name="userPreferences"></param>
        /// <returns></returns>
        Task SaveAsync(UserPreferences userPreferences);
    }
}
