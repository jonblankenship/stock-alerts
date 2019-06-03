using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Domain.Authentication
{
    public interface IAccountsService
    {
        Task RegisterUserAsync(RegisterRequest registerRequest, CancellationToken cancellationToken);

        Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken);

        Task<bool> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken);

        Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken);
    }
}
