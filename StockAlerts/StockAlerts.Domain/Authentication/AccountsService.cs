using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Identity;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;
using StockAlerts.Domain.Settings;

namespace StockAlerts.Domain.Authentication
{
    public class AccountsService : IAccountsService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ISettings _settings;

        public AccountsService(
            UserManager<IdentityUser> userManager,
            IAppUsersRepository appUsersRepository,
            ITokenFactory tokenFactory,
            IJwtTokenFactory jwtTokenFactory,
            ISettings settings)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _appUsersRepository = appUsersRepository ?? throw new ArgumentNullException(nameof(appUsersRepository));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(jwtTokenFactory));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task RegisterUserAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            registerRequest.Validate();

            var user = new IdentityUser
            {
                UserName = registerRequest.Username,
                Email = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any())
                {
                    throw new BadRequestException(result.Errors.First().Description);
                }

                throw new ApplicationException("User creation was unsuccessful.");
            }

            // Create the app user
            var appUser = new AppUser(_appUsersRepository)
            {
                UserId = new Guid(user.Id),
                UserName = user.UserName,
                HasBeenGrantedAccess = true
            };
            await appUser.SaveAsync(cancellationToken);

            // TODO: Send welcome e-mail
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            // ensure we have a user with the given user name
            var user = await _userManager.FindByNameAsync(loginRequest.Username);

            // validate password
            if (await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var appUser = await _appUsersRepository.GetAppUserByUserIdAsync(new Guid(user.Id), CancellationToken.None);

                if (appUser.HasBeenGrantedAccess)
                {
                    // generate refresh token
                    var refreshToken = _tokenFactory.GenerateToken();
                    appUser.AddRefreshToken(refreshToken, new Guid(user.Id), loginRequest.RemoteIpAddress);

                    await appUser.SaveAsync(cancellationToken);

                    // generate access token
                    var accessToken = await _jwtTokenFactory.GenerateEncodedTokenAsync(user.Id, user.UserName, appUser.AppUserId.ToString());

                    return new LoginResponse(accessToken, refreshToken, true);
                }
                else
                {
                    return new LoginResponse(new List<Error> { new Error(LoginResponse.LoginErrorCodes.AwaitingAccess, "Your request for access has been submitted. You'll receive an e-mail when access has been granted. Thank you!") });

                }
            }

            throw new BadRequestException("Username or password is incorrect.");
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken)
        {
            forgotPasswordRequest.Validate();

            var user = await _userManager.FindByEmailAsync(forgotPasswordRequest.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return true;
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            // TODO: Implement _emailSender and Reset Password e-mail template
            // var callbackUrl = $"{_settings.WebAppBaseUrl}/reset-password?code={HttpUtility.UrlEncode(code)}";
            // await _emailSender.SendEmailAsync(user.Email, new ResetPasswordEmailModel(callbackUrl));
            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            resetPasswordRequest.Validate();

            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return true;
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordRequest.Code, resetPasswordRequest.Password);
            if (result.Succeeded)
            {
                return true;
            }

            if (result.Errors.Any())
            {
                throw new ApplicationException(result.Errors.First().Description);
            }

            return false;
        }
    }
}
