using System.Threading.Tasks;
using StockAlerts.Resources.Model.Authentication;

namespace StockAlerts.App.Services.Accounts
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(string username, string password);

        Task<LoginResponse> RegisterAsync(string emailAddress, string username, string password);
    }
}
