using Microsoft.AspNetCore.Mvc;
using StockAlerts.Domain.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StockAlerts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService ?? throw new ArgumentNullException(nameof(accountsService));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
            registerRequest.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _accountsService.RegisterUserAsync(registerRequest, cancellationToken);

            var loginRequest = new LoginRequest(registerRequest);

            return await LoginAsync(loginRequest, cancellationToken);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            loginRequest.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var response = await _accountsService.LoginAsync(loginRequest, cancellationToken);

            return Ok(response);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ForgotPasswordRequest forgotPasswordRequest, CancellationToken cancellationToken)
        {
            forgotPasswordRequest.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var response = await _accountsService.ForgotPasswordAsync(forgotPasswordRequest, cancellationToken);

            return Ok(response);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ForgotPasswordAsync([FromBody] ResetPasswordRequest resetPasswordRequest, CancellationToken cancellationToken)
        {
            resetPasswordRequest.RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ModelState.IsValid == false)
            {
                return BadRequest();
            }

            var response = await _accountsService.ResetPasswordAsync(resetPasswordRequest, cancellationToken);

            return Ok(response);
        }
    }
}
