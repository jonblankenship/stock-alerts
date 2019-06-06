using StockAlerts.Domain.Model;
using System;
using System.Threading.Tasks;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        private readonly IUserPreferencesRepository _userPreferencesRepository;

        public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
        {
            _userPreferencesRepository = userPreferencesRepository ?? throw new ArgumentNullException(nameof(userPreferencesRepository));
        }

        public async Task<UserPreferences> GetUserPreferencesAsync(Guid appUserId)
        {
            var userPreferences = await _userPreferencesRepository.GetUserPreferencesAsync(appUserId);

            if (userPreferences == null)
                throw new NotFoundException($"{nameof(UserPreferences)} for appUserId {appUserId} not found.");

            return userPreferences;
        }
    }
}
