using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using StockAlerts.Forms.Models.Token;
using StockAlerts.Forms.Services.RequestProvider;
using StockAlerts.Domain.Authentication;

namespace StockAlerts.Forms.Services.Account
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
            var loginRequest = new LoginRequest()
            {
                Username = username,
                Password = password
            };

            var result = await _requestProvider.PostAsync<LoginRequest, LoginResponse>("https://stockalerts.azurewebsites.net/api/accounts/login", loginRequest);

            return result;
        }
    }
}
