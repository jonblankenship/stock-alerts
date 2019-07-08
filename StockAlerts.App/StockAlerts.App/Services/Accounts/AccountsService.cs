using System;
using System.Threading;
using System.Threading.Tasks;
using StockAlerts.App.Constants;
using StockAlerts.App.Services.RequestProvider;
using StockAlerts.App.Services.Settings;
using StockAlerts.Resources.Model.Authentication;

namespace StockAlerts.App.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settingsService;

        public AccountService(
            IRequestProvider requestProvider,
            ISettingsService settingsService)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var result = await _requestProvider.PostAsync<LoginRequest, LoginResponse>($"{MiscConstants.StockAlertsApiBaseUri}accounts/login", loginRequest);

            return result;
        }

        public async Task<LoginResponse> RegisterAsync(string emailAddress, string username, string password)
        {
            var registerRequest = new RegisterRequest
            {
                Email = emailAddress,
                Username = username,
                Password = password
            };

            var result = await _requestProvider.PostAsync<RegisterRequest, LoginResponse>($"{MiscConstants.StockAlertsApiBaseUri}accounts/register", registerRequest);

            return result;
        }
    }
}
