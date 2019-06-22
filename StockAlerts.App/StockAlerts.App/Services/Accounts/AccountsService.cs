using System;
using System.Threading.Tasks;
using StockAlerts.App.Services.RequestProvider;
using StockAlerts.Resources.Model.Authentication;

namespace StockAlerts.App.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IRequestProvider _requestProvider;

        public AccountService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            var result = await _requestProvider.PostAsync<LoginRequest, LoginResponse>("https://stockalerts.azurewebsites.net/api/accounts/login", loginRequest);

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

            var result = await _requestProvider.PostAsync<RegisterRequest, LoginResponse>("https://stockalerts.azurewebsites.net/api/accounts/register", registerRequest);

            return result;
        }
    }
}
