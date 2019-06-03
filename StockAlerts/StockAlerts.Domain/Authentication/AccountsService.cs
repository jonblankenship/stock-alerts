using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StockAlerts.Domain.Exceptions;
using StockAlerts.Domain.Model;
using StockAlerts.Domain.Repositories;

namespace StockAlerts.Domain.Authentication
{
    public class AccountsService : IAccountsService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtTokenFactory _jwtTokenFactory;

        public AccountsService(
            UserManager<IdentityUser> userManager,
            IAppUsersRepository appUsersRepository,
            ITokenFactory tokenFactory,
            IJwtTokenFactory jwtTokenFactory)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _appUsersRepository = appUsersRepository ?? throw new ArgumentNullException(nameof(appUsersRepository));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _jwtTokenFactory = jwtTokenFactory ?? throw new ArgumentNullException(nameof(jwtTokenFactory));
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

        public Task<bool> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResetPasswordAsync(ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
