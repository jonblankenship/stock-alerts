using StockAlerts.Domain.Exceptions;

namespace StockAlerts.Domain.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string TwitterHandle { get; set; }

        public string RemoteIpAddress { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                throw new BadRequestException($"{nameof(Username)} is required.");
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new BadRequestException($"{nameof(Email)} is required.");
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                throw new BadRequestException($"{nameof(Password)} is required.");
            }
        }
    }
}
