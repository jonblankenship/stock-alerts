using StockAlerts.Domain.Exceptions;

namespace StockAlerts.Domain.Authentication
{
    public class ResetPasswordRequest
    {
        public ResetPasswordRequest(string email, string password, string code, string remoteIpAddress)
        {
            Email = email;
            Password = password;
            Code = code;
            RemoteIpAddress = remoteIpAddress;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Code { get; set; }

        public string RemoteIpAddress { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new BadRequestException($"{nameof(Email)} is required.");
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new BadRequestException($"{nameof(Password)} is required.");
            }

            if (string.IsNullOrWhiteSpace(Code))
            {
                throw new BadRequestException($"{nameof(Code)} is required.");
            }
        }
    }
}
