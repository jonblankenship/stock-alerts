using StockAlerts.Domain.Authentication;
using System.Threading.Tasks;

namespace StockAlerts.Forms.Services.Account
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(string username, string password);

        Task<LoginResponse> RegisterAsync(string emailAddress, string username, string password);
    }
}
