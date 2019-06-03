using Microsoft.AspNetCore.Identity;
using StockAlerts.Domain.Model;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Authentication
{
    public interface IJwtTokenFactory
    {
        string BuildToken(IdentityUser user, AppUser appUser);

        Task<AccessToken> GenerateEncodedTokenAsync(string id, string userName, string appUserId);
    }
}
